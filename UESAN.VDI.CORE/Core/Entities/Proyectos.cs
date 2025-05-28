using System;
using System.Collections.Generic;

namespace UESAN.VDI.CORE.Core.Entities;

public partial class Proyectos
{
    public int ProyectoId { get; set; }

    public string Titulo { get; set; } = null!;

    public string? Descripcion { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime? FechaFin { get; set; }

    public string Estatus { get; set; } = null!;

    public bool Recomendado { get; set; }

    public int AdminCrea { get; set; }

    public int? LineaId { get; set; }

    public bool Activo { get; set; } = true;

    public virtual Usuarios AdminCreaNavigation { get; set; } = null!;

    public virtual ICollection<AsignacionProyecto> AsignacionProyecto { get; set; } = new List<AsignacionProyecto>();

    public virtual ICollection<FormulariosInvestigacion> FormulariosInvestigacion { get; set; } = new List<FormulariosInvestigacion>();

    public virtual LineasInvestigacion? Linea { get; set; }
}
