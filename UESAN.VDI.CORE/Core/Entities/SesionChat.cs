using System;
using System.Collections.Generic;

namespace UESAN.VDI.CORE.Core.Entities;

public partial class SesionChat
{
    public int SesionId { get; set; }

    public int UsuarioId { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime? FechaFin { get; set; }

    public bool Activo { get; set; } = true;

    public virtual ICollection<MensajeChat> MensajeChat { get; set; } = new List<MensajeChat>();

    public virtual Usuarios Usuario { get; set; } = null!;
}
