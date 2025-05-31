using System;

namespace UESAN.VDI.CORE.Core.DTOs
{
    public class AutorPublicacionDTO
    {
        public int Id { get; set; }
        public int PublicacionId { get; set; }
        public int ProfesorId { get; set; }
        public int OrdenAutor { get; set; }
        public decimal? PorcentajeParticipacion { get; set; }
    }

    public class AutorPublicacionCreateDTO
    {
        public int PublicacionId { get; set; }
        public int ProfesorId { get; set; }
        public int OrdenAutor { get; set; }
        public decimal? PorcentajeParticipacion { get; set; }
    }

    public class AutorPublicacionListDTO
    {
        public int Id { get; set; }
        public int OrdenAutor { get; set; }
    }
}
