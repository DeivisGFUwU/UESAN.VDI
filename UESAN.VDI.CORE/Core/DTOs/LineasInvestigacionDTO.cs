using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UESAN.VDI.CORE.Core.DTOs
{
    public class LineasInvestigacionDTO
    {
        public int LineaId { get; set; }
        public string Nombre { get; set; } = null!;
    }

    public class LineasInvestigacionListDTO
    {
        public int LineaId { get; set; }
        public string Nombre { get; set; } = null!;
    }

    public class LineasInvestigacionCreateDTO
    {
        public string Nombre { get; set; } = null!;
    }
}
