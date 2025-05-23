using System;
using System.Collections.Generic;

namespace UESAN.VDI.CORE.Core.Entities;

public partial class Profesores
{
    public int ProfesorId { get; set; }

    public int UsuarioId { get; set; }

    public string? Departamento { get; set; }

    public DateTime? FechaIngreso { get; set; }

    public string? Categoria { get; set; }

    public bool Activo { get; set; }

    public virtual ICollection<AsignacionProyecto> AsignacionProyecto { get; set; } = new List<AsignacionProyecto>();

    public virtual ICollection<AutoresPublicacion> AutoresPublicacion { get; set; } = new List<AutoresPublicacion>();

    public virtual ICollection<Publicaciones> Publicaciones { get; set; } = new List<Publicaciones>();

    public virtual Usuarios Usuario { get; set; } = null!;
}
