using System;
using System.Collections.Generic;

namespace UESAN.VDI.CORE.Core.Entities;

public partial class AutoresPublicacion
{
    public int Id { get; set; }

    public int PublicacionId { get; set; }

    public int ProfesorId { get; set; }

    public int OrdenAutor { get; set; }

    public decimal? PorcentajeParticipacion { get; set; }

    public bool Activo { get; set; } = true;

    public virtual Profesores Profesor { get; set; } = null!;

    public virtual Publicaciones Publicacion { get; set; } = null!;
}
