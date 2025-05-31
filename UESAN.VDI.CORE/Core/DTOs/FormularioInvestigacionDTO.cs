using System;

namespace UESAN.VDI.CORE.Core.DTOs
{
    public class FormularioInvestigacionDTO
    {
        public int FormularioId { get; set; }
        public int ProyectoId { get; set; }
        public string? TipoFormulario { get; set; }
        public string? Resumen { get; set; }
        public string? Objetivos { get; set; }
        public string? MedioPublicacion { get; set; }
        public string? Issn { get; set; }
        public string? Doi { get; set; }
        public decimal? Presupuesto { get; set; }
        public string? Cronograma { get; set; }
    }

    public class FormularioInvestigacionCreateDTO
    {
        public int ProyectoId { get; set; }
        public string? TipoFormulario { get; set; }
        public string? Resumen { get; set; }
        public string? Objetivos { get; set; }
        public string? MedioPublicacion { get; set; }
        public string? Issn { get; set; }
        public string? Doi { get; set; }
        public decimal? Presupuesto { get; set; }
        public string? Cronograma { get; set; }
    }

    public class FormularioInvestigacionListDTO
    {
        public int FormularioId { get; set; }
        public string? TipoFormulario { get; set; }
        public string? Resumen { get; set; }
    }
}
