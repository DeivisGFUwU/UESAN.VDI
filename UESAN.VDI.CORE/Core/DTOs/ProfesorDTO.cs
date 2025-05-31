using System;

namespace UESAN.VDI.CORE.Core.DTOs
{
    public class ProfesorDTO
    {
        public int ProfesorId { get; set; }
        public int UsuarioId { get; set; }
        public string? Departamento { get; set; }
        public DateTime? FechaIngreso { get; set; }
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
}
