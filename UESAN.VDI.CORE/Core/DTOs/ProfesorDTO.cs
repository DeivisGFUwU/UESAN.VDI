using System;
using System.Collections.Generic;

namespace UESAN.VDI.CORE.Core.DTOs
{
    public class ProfesorDTO
    {
        public int ProfesorId { get; set; }
        public string? Departamento { get; set; }
        public string? Categoria { get; set; }
    }

    public class ProfesorCreateDTO
    {
        public int UsuarioId { get; set; }
        public string? Departamento { get; set; }
        public DateTime? FechaIngreso { get; set; }
        public string? Categoria { get; set; }
    }

    public class ProfesorListDTO
    {
        public int ProfesorId { get; set; }
        public string? Departamento { get; set; }
        public string? Categoria { get; set; }
    }

    public class ProfesorUsuarioCreateDTO
    {
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public string? Departamento { get; set; }
        public string? Categoria { get; set; }
        public string? Password { get; set; }
    }

    public class ProfesoresMasivoResultadoDTO
    {
        public List<string> ProfesoresAsignados { get; set; } = new List<string>();
        public List<string> ProfesoresNoAsignados { get; set; } = new List<string>();
    }
}
