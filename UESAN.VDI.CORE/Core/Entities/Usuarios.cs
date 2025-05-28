using System;
using System.Collections.Generic;

namespace UESAN.VDI.CORE.Core.Entities;

public partial class Usuarios
{
    public int UsuarioId { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string ClaveHash { get; set; } = null!;

    public int RoleId { get; set; }

    public bool CorreoVerificado { get; set; }

    public DateTime FechaRegistro { get; set; }

    public bool Activo { get; set; }

    public virtual Profesores? Profesores { get; set; }

    public virtual ICollection<Proyectos> Proyectos { get; set; } = new List<Proyectos>();

    public virtual Roles Role { get; set; } = null!;

    public virtual ICollection<SesionChat> SesionChat { get; set; } = new List<SesionChat>();
}
