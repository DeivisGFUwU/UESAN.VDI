using System;
using System.Collections.Generic;

namespace UESAN.VDI.CORE.Core.Entities;

public partial class LineasInvestigacion
{
    public int LineaId { get; set; }

    public string Nombre { get; set; } = null!;

    public bool Activo { get; set; } = true;

    public virtual ICollection<Proyectos> Proyectos { get; set; } = new List<Proyectos>();
}
