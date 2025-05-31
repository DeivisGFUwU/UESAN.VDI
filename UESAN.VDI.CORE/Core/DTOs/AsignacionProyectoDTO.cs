using System;

namespace UESAN.VDI.CORE.Core.DTOs
{
    public class AsignacionProyectoDTO
    {
        public int AsignacionId { get; set; }
        public int ProyectoId { get; set; }
        public int ProfesorId { get; set; }
        public DateTime FechaAsignacion { get; set; }
        public string? RolEnProyecto { get; set; }
    }

    public class AsignacionProyectoCreateDTO
    {
        public int ProyectoId { get; set; }
        public int ProfesorId { get; set; }
        public string? RolEnProyecto { get; set; }
    }

    public class AsignacionProyectoListDTO
    {
        public int AsignacionId { get; set; }
        public string? RolEnProyecto { get; set; }
    }
}
