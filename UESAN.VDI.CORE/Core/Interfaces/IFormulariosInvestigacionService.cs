using System.Collections.Generic;
using System.Threading.Tasks;
using UESAN.VDI.CORE.Core.DTOs;

namespace UESAN.VDI.CORE.Core.Interfaces
{
    public interface IFormulariosInvestigacionService
    {
        Task<List<FormularioInvestigacionListDTO>> GetAllAsync();
        Task<FormularioInvestigacionDTO?> GetByIdAsync(int id);
        Task<int> CreateAsync(FormularioInvestigacionCreateDTO dto);
        Task<bool> UpdateAsync(int id, FormularioInvestigacionDTO dto);
        Task<bool> SoftDeleteAsync(int id);
    }
}
