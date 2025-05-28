using System;

namespace UESAN.VDI.CORE.Core.DTOs
{
    public class RevistaDTO
    {
        public string Issn { get; set; } = null!;
        public string Titulo { get; set; } = null!;
        public string? Categoria { get; set; }
        public string? Cuartil { get; set; }
        public bool Activa { get; set; }
        public string? Pais { get; set; }
    }

    public class RevistaListDTO
    {
        public string Issn { get; set; } = null!;
        public string Titulo { get; set; } = null!;
    }
}
