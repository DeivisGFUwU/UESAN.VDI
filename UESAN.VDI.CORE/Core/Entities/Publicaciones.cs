using System;
using System.Collections.Generic;

namespace UESAN.VDI.CORE.Core.Entities;

public partial class Publicaciones
{
    public int PublicacionId { get; set; }

    public int ProfesorId { get; set; }

    public string Issn { get; set; } = null!;

    public string Titulo { get; set; } = null!;

    public DateTime FechaPublicacion { get; set; }

    public int Puntaje { get; set; }

    public decimal Incentivo { get; set; }

    public string? Doi { get; set; }

    public bool Activo { get; set; } = true;

    public virtual ICollection<AutoresPublicacion> AutoresPublicacion { get; set; } = new List<AutoresPublicacion>();

    public virtual Revistas IssnNavigation { get; set; } = null!;

    public virtual Profesores Profesor { get; set; } = null!;
}
