using System;

namespace UESAN.VDI.CORE.Core.DTOs
{
    public class PublicacionDTO
    {
        public int PublicacionId { get; set; }
        public int ProfesorId { get; set; }
        public string Issn { get; set; } = null!;
        public string Titulo { get; set; } = null!;
        public DateTime FechaPublicacion { get; set; }
        public int Puntaje { get; set; }
    }

    public class PublicacionCreateDTO
    {
        public int ProfesorId { get; set; }
        public string Issn { get; set; } = null!;
        public string Titulo { get; set; } = null!;
        public DateTime FechaPublicacion { get; set; }
        public int Puntaje { get; set; }
    }

    public class PublicacionListDTO
    {
        public int PublicacionId { get; set; }
        public string Titulo { get; set; } = null!;
        public DateTime FechaPublicacion { get; set; }
    }
}
