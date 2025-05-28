using System;

namespace UESAN.VDI.CORE.Core.DTOs
{
    public class SesionChatDTO
    {
        public int SesionId { get; set; }
        public int UsuarioId { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
    }

    public class SesionChatListDTO
    {
        public int SesionId { get; set; }
        public DateTime FechaInicio { get; set; }
    }
}
