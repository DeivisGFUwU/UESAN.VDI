using System;
using System.Collections.Generic;

namespace UESAN.VDI.CORE.Core.Entities;

public partial class AsignacionProyecto
{
    public int AsignacionId { get; set; }

    public int ProyectoId { get; set; }

    public int ProfesorId { get; set; }

    public DateTime FechaAsignacion { get; set; }

    public string? RolEnProyecto { get; set; }

    public bool Activo { get; set; } = true;

    public virtual Profesores Profesor { get; set; } = null!;

    public virtual Proyectos Proyecto { get; set; } = null!;
}
