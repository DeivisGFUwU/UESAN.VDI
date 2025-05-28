using System;
using System.Collections.Generic;

namespace UESAN.VDI.CORE.Core.Entities;

public partial class FormulariosInvestigacion
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

    public DateTime? FechaRegistro { get; set; }

    public bool Activo { get; set; } = true;

    public virtual Revistas? IssnNavigation { get; set; }

    public virtual Proyectos Proyecto { get; set; } = null!;
}
