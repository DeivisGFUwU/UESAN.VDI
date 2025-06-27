using System;

namespace UESAN.VDI.CORE.Core.DTOs
{
    public class UsuarioWithPasswordDTO
    {
        public int UsuarioId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty; // Contraseña en texto plano
    }
}
