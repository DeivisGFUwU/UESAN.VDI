using UESAN.VDI.CORE.Core.Entities;

namespace UESAN.VDI.CORE.Core.Interfaces
{
    public interface ILineasInvestigacionRepository
    {
        Task<int> CreateLineasInvestigacion(LineasInvestigacion lineaInvestigacion);
        Task DeleteLineasInvestigacion(int id);
        Task<List<LineasInvestigacion>> GetAllLineasInvestigacion();
        Task<LineasInvestigacion> GetLineasInvestigacionById(int id);
        Task<bool> UpdateLineasInvestigacion(LineasInvestigacion lineaInvestigacion);
    }
}