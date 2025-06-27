using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UESAN.VDI.CORE.Core.DTOs;
using UESAN.VDI.CORE.Core.Entities;
using UESAN.VDI.CORE.Core.Interfaces;
using UESAN.VDI.CORE.Core.Helpers;
using BCrypt.Net;

namespace UESAN.VDI.CORE.Core.Services
{
    public class UsuariosService : IUsuariosService
    {
        private readonly IUsuariosRepository _usuariosRepository;
        private readonly IProfesoresRepository _profesoresRepository;
        private readonly IJWTService _jwtService;
        private readonly IEmailService _emailService;

        public UsuariosService(IUsuariosRepository usuariosRepository, IProfesoresRepository profesoresRepository, IJWTService jwtService, IEmailService emailService)
        {
            _usuariosRepository = usuariosRepository;
            _profesoresRepository = profesoresRepository;
            _jwtService = jwtService;
            _emailService = emailService;
        }

        public async Task<UsuarioSignInResponseDTO?> SignInAsync(UsuarioSignInRequestDTO dto)
        {
            var user = await _usuariosRepository.GetByCorreoAsync(dto.Correo);
            if (user == null)
            {
                Console.WriteLine("Correo no encontrado");
                return null;
            }

            var match = BCrypt.Net.BCrypt.Verify(dto.Password, user.ClaveHash);
            if (!match)
            {
                Console.WriteLine("Contraseña incorrecta");
                return null;
            }

            var jwt = _jwtService.GenerateJWToken(user);
            return new UsuarioSignInResponseDTO
            {
                UsuarioId = user.UsuarioId,
                Nombre = user.Nombre,
                Apellido = user.Apellido,
                Correo = user.Correo,
                RoleId = user.RoleId,
                Jwt = jwt
            };
        }

        public async Task<List<UsuarioListDTO>> GetAllAsync(string? userRole, bool includeInactive = false)
        {
            var users = includeInactive && RoleHelper.IsAdmin(userRole)
                ? await _usuariosRepository.GetAllAsync()
                : await _usuariosRepository.GetAllActivosAsync();
            return users.Select(u => new UsuarioListDTO
            {
                UsuarioId = u.UsuarioId,
                Nombre = u.Nombre,
                Apellido = u.Apellido,
                Correo = u.Correo
            }).ToList();
        }

        public async Task<UsuarioDTO?> GetByIdAsync(int id, string? userRole)
        {
            var user = await _usuariosRepository.GetByIdAsync(id);
            if (user == null) return null;
            // Nunca exponer ClaveHash ni datos sensibles
            return new UsuarioDTO
            {
                UsuarioId = user.UsuarioId,
                Nombre = user.Nombre,
                Apellido = user.Apellido,
                Correo = user.Correo,
                RoleId = RoleHelper.IsAdmin(userRole) ? user.RoleId : 0, // Solo admin ve el rol
                CorreoVerificado = user.CorreoVerificado
            };
        }

        public async Task<List<UsuarioListDTO>> GetAllActivosAsync()
        {
            var users = await _usuariosRepository.GetAllActivosAsync();
            return users.Select(u => new UsuarioListDTO
            {
                UsuarioId = u.UsuarioId,
                Nombre = u.Nombre,
                Apellido = u.Apellido,
                Correo = u.Correo
            }).ToList();
        }

        public async Task<UsuarioDTO?> GetByIdAsync(int id)
        {
            var user = await _usuariosRepository.GetByIdAsync(id);
            if (user == null) return null;
            return new UsuarioDTO
            {
                UsuarioId = user.UsuarioId,
                Nombre = user.Nombre,
                Apellido = user.Apellido,
                Correo = user.Correo,
                RoleId = 0, // No exponer rol por defecto
                CorreoVerificado = user.CorreoVerificado
            };
        }

        public async Task<bool> ReactivateAsync(int usuarioId)
        {
            // Reactivar usuario
            var user = await _usuariosRepository.GetByIdAsync(usuarioId, includeInactive: true);
            if (user == null || user.Activo) return false;
            user.Activo = true;
            await _usuariosRepository.UpdateAsync(user);

            // Reactivar profesor si existe
            var profesor = await _profesoresRepository.GetByUsuarioIdAsync(usuarioId);
            if (profesor != null && !profesor.Activo)
            {
                profesor.Activo = true;
                await _profesoresRepository.UpdateAsync(profesor);
            }
            return true;
        }

        public async Task<bool> SoftDeleteAsync(int usuarioId)
        {
            // Soft delete usuario
            var deleted = await _usuariosRepository.SoftDeleteAsync(usuarioId);
            if (!deleted) return false;

            // Soft delete profesor vinculado
            var profesor = await _profesoresRepository.GetByUsuarioIdAsync(usuarioId);
            if (profesor != null && profesor.Activo)
            {
                profesor.Activo = false;
                await _profesoresRepository.UpdateAsync(profesor);
            }
            return true;
        }

        public async Task<int> CreateAsync(UsuarioCreateDTO dto)
        {
            var correoVerificado = dto.Correo.EndsWith("@esan.edu.pe", StringComparison.OrdinalIgnoreCase);
            var usuario = new Usuarios
            {
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Correo = dto.Correo,
                RoleId = dto.RoleId,
                CorreoVerificado = correoVerificado,
                ClaveHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Activo = true
            };
            var id = await _usuariosRepository.CreateAsync(usuario);
            // Enviar correo con la contraseña en texto plano
            try
            {
                await _emailService.SendEmailAsync(dto.Correo, "Bienvenido a la plataforma VDI", $"Su contraseña es: {dto.Password}");
            }
            catch
            {
                // Ignorar error de envío de correo para no afectar el registro
            }
            return id;
        }

        public async Task<UsuarioWithPasswordDTO?> GetByCorreoAsync(string correo)
        {
            var user = await _usuariosRepository.GetByCorreoAsync(correo);
            if (user == null) return null;
            // Obtener la contraseña en texto plano si está disponible (para prácticas)
            // Aquí asumimos que el campo ClaveHash almacena la contraseña en texto plano para prácticas
            return new UsuarioWithPasswordDTO
            {
                UsuarioId = user.UsuarioId,
                Nombre = user.Nombre,
                Apellido = user.Apellido,
                Correo = user.Correo,
                Password = user.ClaveHash // Para prácticas, guardar la contraseña en texto plano aquí
            };
        }

        public async Task<(bool Success, string? ErrorMessage)> CambiarClaveAsync(int usuarioId, CambiarClaveDTO dto)
        {
            var usuario = await _usuariosRepository.GetByIdAsync(usuarioId);
            if (usuario == null)
                return (false, "Usuario no encontrado");

            // Verificar la contraseña actual
            if (!BCrypt.Net.BCrypt.Verify(dto.ClaveActual, usuario.ClaveHash))
                return (false, "La contraseña actual es incorrecta");

            // Actualizar a la nueva contraseña
            usuario.ClaveHash = BCrypt.Net.BCrypt.HashPassword(dto.ClaveNueva);
            var updated = await _usuariosRepository.UpdateAsync(usuario);
            
            if (!updated)
                return (false, "Error al actualizar la contraseña");

            return (true, null);
        }

        public async Task<(bool Success, string? ErrorMessage)> RecuperarClaveAsync(string correo)
        {
            var usuario = await _usuariosRepository.GetByCorreoAsync(correo);
            if (usuario == null)
                return (false, "Si el correo existe, se han enviado instrucciones para recuperar la contraseña.");

            // Generar nueva contraseña temporal
            var nuevaClave = Guid.NewGuid().ToString("N").Substring(0, 10) + "*aA1";
            usuario.ClaveHash = BCrypt.Net.BCrypt.HashPassword(nuevaClave);
            var updated = await _usuariosRepository.UpdateAsync(usuario);
            if (!updated)
                return (false, "No se pudo actualizar la contraseña. Intente nuevamente.");

            // Enviar correo con la nueva contraseña
            try
            {
                await _emailService.SendEmailAsync(usuario.Correo, "Recuperación de contraseña", $"Su nueva contraseña temporal es: {nuevaClave}");
            }
            catch
            {
                return (false, "No se pudo enviar el correo de recuperación. Contacte al administrador.");
            }

            return (true, null);
        }
    }
}
