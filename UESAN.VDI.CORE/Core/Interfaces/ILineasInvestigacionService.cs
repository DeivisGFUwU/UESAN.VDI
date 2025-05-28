using UESAN.VDI.CORE.Core.DTOs;

namespace UESAN.VDI.CORE.Core.Interfaces
{
    public interface ILineasInvestigacionService
    {
        Task<int> CreateLineasInvestigacion(LineasInvestigacionCreateDTO lineaInvestigacionDto);
        Task<List<LineasInvestigacionDTO>> GetlineasInvestigacion();
        Task<LineasInvestigacionDTO?> GetLineasInvestigacionById(int id);
        Task<bool> UpdateLineasInvestigacion(LineasInvestigacionListDTO lineasInvestigacionDto);
        Task DeleteLineasInvestigacion(int id);
    }
}