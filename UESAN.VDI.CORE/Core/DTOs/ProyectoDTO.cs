using System;

namespace UESAN.VDI.CORE.Core.DTOs
{
    public class ProyectoDTO
    {
        public int ProyectoId { get; set; }
        public string Titulo { get; set; } = null!;
        public string? Descripcion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string Estatus { get; set; } = null!;
        public bool Recomendado { get; set; }
        public int? LineaId { get; set; }
    }

    public class ProyectoCreateDTO
    {
        public string Titulo { get; set; } = null!;
        public string? Descripcion { get; set; }
        public DateTime FechaInicio { get; set; }
        public string Estatus { get; set; } = null!;
        public bool Recomendado { get; set; }
        public int? LineaId { get; set; }
    }

    public class ProyectoListDTO
    {
        public int ProyectoId { get; set; }
        public string Titulo { get; set; } = null!;
        public string Estatus { get; set; } = null!;
    }
}
