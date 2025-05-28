using System;
using System.Collections.Generic;

namespace UESAN.VDI.CORE.Core.Entities;

public partial class MensajeChat
{
    public int MensajeId { get; set; }

    public int SesionId { get; set; }

    public string Remitente { get; set; } = null!;

    public string Texto { get; set; } = null!;

    public DateTime FechaEnvio { get; set; }

    public bool Activo { get; set; } = true;

    public virtual SesionChat Sesion { get; set; } = null!;
}
