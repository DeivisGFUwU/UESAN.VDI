using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UESAN.VDI.CORE.Core.DTOs;
using UESAN.VDI.CORE.Core.Entities;
using UESAN.VDI.CORE.Core.Interfaces;

namespace UESAN.VDI.CORE.Core.Services
{
    public class ProfesoresService : IProfesoresService
    {
        private readonly IProfesoresRepository _profesoresRepository;
        private readonly IUsuariosRepository _usuariosRepository;
        private readonly IUsuariosService _usuariosService;
        private readonly IEmailService _emailService;

        public ProfesoresService(IProfesoresRepository profesoresRepository, IUsuariosRepository usuariosRepository, IUsuariosService usuariosService, IEmailService emailService)
        {
            _profesoresRepository = profesoresRepository;
            _usuariosRepository = usuariosRepository;
            _usuariosService = usuariosService;
            _emailService = emailService;
        }

        public async Task<List<ProfesorListDTO>> GetAllActivosAsync()
        {
            var profesores = await _profesoresRepository.GetAllActivosAsync();
            return profesores.Select(p => new ProfesorListDTO
            {
                ProfesorId = p.ProfesorId,
                Departamento = p.Departamento,
                Categoria = p.Categoria
            }).ToList();
        }

        public async Task<ProfesorDTO?> GetByIdAsync(int id)
        {
            var profesor = await _profesoresRepository.GetByIdAsync(id);
            if (profesor == null) return null;
            return new ProfesorDTO
            {
                ProfesorId = profesor.ProfesorId,
                Departamento = profesor.Departamento,
                Categoria = profesor.Categoria
            };
        }

        public async Task<int> CreateAsync(ProfesorCreateDTO dto)
        {
            var usuario = await _usuariosRepository.GetByIdAsync(dto.UsuarioId);
            if (usuario == null)
                throw new System.Exception($"No existe un usuario con el id {dto.UsuarioId}");
            var profesor = new Profesores
            {
                UsuarioId = dto.UsuarioId,
                Departamento = dto.Departamento,
                FechaIngreso = dto.FechaIngreso,
                Categoria = dto.Categoria,
                Activo = true
            };
            return await _profesoresRepository.CreateAsync(profesor);
        }

        public async Task<bool> UpdateAsync(int id, ProfesorDTO dto)
        {
            var profesor = await _profesoresRepository.GetByIdAsync(id);
            if (profesor == null) return false;
            profesor.Departamento = dto.Departamento;
            profesor.Categoria = dto.Categoria;
            return await _profesoresRepository.UpdateAsync(profesor);
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            var profesor = await _profesoresRepository.GetByIdAsync(id);
            if (profesor == null) return false;
            profesor.Activo = false;
            await _profesoresRepository.UpdateAsync(profesor);
            // También desactivar el usuario
            var usuario = await _usuariosRepository.GetByIdAsync(profesor.UsuarioId);
            if (usuario != null && usuario.Activo)
            {
                usuario.Activo = false;
                await _usuariosRepository.UpdateAsync(usuario);
            }
            return true;
        }

        public async Task<bool> ReactivateAsync(int id)
        {
            var profesor = await _profesoresRepository.GetByIdAsync(id);
            if (profesor == null) return false;
            profesor.Activo = true;
            await _profesoresRepository.UpdateAsync(profesor);
            // También reactivar el usuario
            var usuario = await _usuariosRepository.GetByIdAsync(profesor.UsuarioId);
            if (usuario != null && !usuario.Activo)
            {
                usuario.Activo = true;
                await _usuariosRepository.UpdateAsync(usuario);
            }
            return true;
        }

        public async Task<int> CrearMasivoAsync(List<ProfesorCreateDTO> profesores)
        {
            int cantidad = 0;
            foreach (var profesor in profesores)
            {
                await CreateAsync(profesor);
                cantidad++;
            }
            return cantidad;
        }

        public async Task<bool> CrearProfesorConUsuarioAsync(ProfesorUsuarioCreateDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Correo))
                return false;
            var existeUsuario = await _usuariosRepository.GetByCorreoAsync(dto.Correo);
            if (existeUsuario != null)
                return false;

            // Validar unicidad de profesor por nombre y apellido (ignorando mayúsculas/minúsculas y espacios)
            var usuariosActivos = await _usuariosRepository.GetAllActivosAsync();
            var existeProfesor = usuariosActivos.Any(u => 
                string.Equals(u.Nombre.Trim(), dto.Nombre.Trim(), StringComparison.OrdinalIgnoreCase) &&
                string.Equals(u.Apellido.Trim(), dto.Apellido.Trim(), StringComparison.OrdinalIgnoreCase) &&
                u.Profesores != null && u.Profesores.Activo);
            if (existeProfesor)
                return false;

            // Generar o usar la contraseña en texto plano
            var password = string.IsNullOrWhiteSpace(dto.Password) ? GenerarPasswordSeguro() : dto.Password;
            dto.Password = password; // Guardar la contraseña generada en el DTO
            int usuarioId = 0;
            try
            {
                // Intentar enviar el correo antes de crear usuario y profesor
                await _emailService.SendEmailAsync(dto.Correo, "Bienvenido a la plataforma VDI", $"Su contraseña temporal es: {password}");
                // Si el correo se envía correctamente, crear usuario y profesor
                usuarioId = await _usuariosService.CreateAsync(new UsuarioCreateDTO
                {
                    Nombre = dto.Nombre,
                    Apellido = dto.Apellido,
                    Correo = dto.Correo,
                    RoleId = dto.RoleId,
                    Password = password
                });
                var profesor = new Profesores
                {
                    UsuarioId = usuarioId,
                    Departamento = dto.Departamento,
                    FechaIngreso = DateTime.UtcNow,
                    Categoria = dto.Categoria,
                    Activo = true
                };
                await _profesoresRepository.CreateAsync(profesor);
            }
            catch
            {
                // Si falla el correo, no crear usuario ni profesor
                return false;
            }
            return true;
        }

        public async Task<(bool Success, string? ErrorMessage)> CrearProfesorConUsuarioDebugAsync(ProfesorUsuarioCreateDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Correo))
                return (false, "El correo es obligatorio.");
            var existeUsuario = await _usuariosRepository.GetByCorreoAsync(dto.Correo);
            if (existeUsuario != null)
                return (false, "Ya existe un usuario activo con ese correo.");

            // Validar unicidad de profesor por nombre y apellido (ignorando mayúsculas/minúsculas y espacios)
            var usuariosActivos = await _usuariosRepository.GetAllActivosAsync();
            var existeProfesor = usuariosActivos.Any(u => 
                string.Equals(u.Nombre.Trim(), dto.Nombre.Trim(), StringComparison.OrdinalIgnoreCase) &&
                string.Equals(u.Apellido.Trim(), dto.Apellido.Trim(), StringComparison.OrdinalIgnoreCase) &&
                u.Profesores != null && u.Profesores.Activo);
            if (existeProfesor)
                return (false, "Ya existe un profesor activo con ese nombre y apellido.");

            // Generar o usar la contraseña en texto plano
            var password = string.IsNullOrWhiteSpace(dto.Password) ? GenerarPasswordSeguro() : dto.Password;
            dto.Password = password; // Guardar la contraseña generada en el DTO
            int usuarioId = 0;
            try
            {
                // Intentar enviar el correo antes de crear usuario y profesor
                await _emailService.SendEmailAsync(dto.Correo, "Bienvenido a la plataforma VDI", $"Su contraseña temporal es: {password}");
                // Si el correo se envía correctamente, crear usuario y profesor
                usuarioId = await _usuariosService.CreateAsync(new UsuarioCreateDTO
                {
                    Nombre = dto.Nombre,
                    Apellido = dto.Apellido,
                    Correo = dto.Correo,
                    RoleId = dto.RoleId,
                    Password = password
                });
                var profesor = new Profesores
                {
                    UsuarioId = usuarioId,
                    Departamento = dto.Departamento,
                    FechaIngreso = DateTime.UtcNow,
                    Categoria = dto.Categoria,
                    Activo = true
                };
                await _profesoresRepository.CreateAsync(profesor);
            }
            catch (Exception ex)
            {
                // Si falla el correo, no crear usuario ni profesor
                return (false, $"Error al enviar correo o crear usuario/profesor: {ex.Message}");
            }
            return (true, null);
        }

        public async Task<ProfesoresMasivoResultadoDTO> CrearProfesoresConUsuariosMasivoAsync(List<ProfesorUsuarioCreateDTO> dtos)
        {
            var resultado = new ProfesoresMasivoResultadoDTO();
            foreach (var dto in dtos)
            {
                try
                {
                    var ok = await CrearProfesorConUsuarioAsync(dto);
                    if (ok)
                        resultado.ProfesoresAsignados.Add($"{dto.Nombre} {dto.Apellido}");
                    else
                        resultado.ProfesoresNoAsignados.Add($"{dto.Nombre} {dto.Apellido}");
                }
                catch
                {
                    resultado.ProfesoresNoAsignados.Add($"{dto.Nombre} {dto.Apellido}");
                }
            }
            return resultado;
        }

        private string GenerarPasswordSeguro()
        {
            var guid = Guid.NewGuid().ToString("N");
            return guid.Substring(0, 10) + "*aA1";
        }
    }
}
