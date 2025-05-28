using System;

namespace UESAN.VDI.CORE.Core.DTOs
{
    public class MensajeChatDTO
    {
        public int MensajeId { get; set; }
        public int SesionId { get; set; }
        public string Remitente { get; set; } = null!;
        public string Texto { get; set; } = null!;
        public DateTime FechaEnvio { get; set; }
    }

    public class MensajeChatListDTO
    {
        public int MensajeId { get; set; }
        public string Remitente { get; set; } = null!;
        public DateTime FechaEnvio { get; set; }
    }
}
