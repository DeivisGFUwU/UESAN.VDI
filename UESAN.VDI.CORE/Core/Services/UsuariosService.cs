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
        private readonly IJWTService _jwtService;

        public UsuariosService(IUsuariosRepository usuariosRepository, IJWTService jwtService)
        {
            _usuariosRepository = usuariosRepository;
            _jwtService = jwtService;
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
    }
}
