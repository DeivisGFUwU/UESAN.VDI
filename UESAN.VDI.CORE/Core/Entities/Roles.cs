using System;
using System.Collections.Generic;

namespace UESAN.VDI.CORE.Core.Entities;

public partial class Roles
{
    public int RoleId { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Usuarios> Usuarios { get; set; } = new List<Usuarios>();
}
