using System.Collections.Generic;
using System.Threading.Tasks;
using UESAN.VDI.CORE.Core.Entities;

namespace UESAN.VDI.CORE.Core.Interfaces
{
    public interface IFormulariosInvestigacionRepository
    {
        Task<List<FormulariosInvestigacion>> GetAllAsync();
        Task<FormulariosInvestigacion?> GetByIdAsync(int id);
        Task<int> CreateAsync(FormulariosInvestigacion entity);
        Task<bool> UpdateAsync(FormulariosInvestigacion entity);
        Task<bool> SoftDeleteAsync(int id);
    }
}
