using System;

namespace UESAN.VDI.CORE.Core.DTOs
{
    public class UsuarioDTO
    {
        public int UsuarioId { get; set; }
        public string Nombre { get; set; } = null!;
        public string Apellido { get; set; } = null!;
        public string Correo { get; set; } = null!;
        public int RoleId { get; set; }
        public bool CorreoVerificado { get; set; }
    }

    public class UsuarioListDTO
    {
        public int UsuarioId { get; set; }
        public string Nombre { get; set; } = null!;
        public string Apellido { get; set; } = null!;
        public string Correo { get; set; } = null!;
    }

    public class UsuarioSignInRequestDTO
    {
        public string Correo { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class UsuarioSignInResponseDTO
    {
        public int UsuarioId { get; set; }
        public string Nombre { get; set; } = null!;
        public string Apellido { get; set; } = null!;
        public string Correo { get; set; } = null!;
        public int RoleId { get; set; }
        public string Jwt { get; set; } = null!;
    }

    public class UsuarioCreateDTO
    {
        public string Nombre { get; set; } = null!;
        public string Apellido { get; set; } = null!;
        public string Correo { get; set; } = null!;
        public int RoleId { get; set; }
        public string Password { get; set; } = null!;
    }
}
