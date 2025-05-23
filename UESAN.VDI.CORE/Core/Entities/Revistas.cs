using System;
using System.Collections.Generic;

namespace UESAN.VDI.CORE.Core.Entities;

public partial class Revistas
{
    public string Issn { get; set; } = null!;

    public string Titulo { get; set; } = null!;

    public string? Categoria { get; set; }

    public string? Cuartil { get; set; }

    public bool EnListaCerrada { get; set; }

    public bool EsNueva { get; set; }

    public bool Activa { get; set; }

    public virtual ICollection<FormulariosInvestigacion> FormulariosInvestigacion { get; set; } = new List<FormulariosInvestigacion>();

    public virtual ICollection<Publicaciones> Publicaciones { get; set; } = new List<Publicaciones>();
}
