using System.Collections.Generic;
using System.Threading.Tasks;
using UESAN.VDI.CORE.Core.Entities;

namespace UESAN.VDI.CORE.Core.Interfaces
{
    public interface IAutoresPublicacionRepository
    {
        Task<List<AutoresPublicacion>> GetAllAsync();
        Task<AutoresPublicacion?> GetByIdAsync(int id);
        Task<int> CreateAsync(AutoresPublicacion entity);
        Task<bool> UpdateAsync(AutoresPublicacion entity);
        Task<bool> SoftDeleteAsync(int id); // Agregado para soft delete
    }
}
