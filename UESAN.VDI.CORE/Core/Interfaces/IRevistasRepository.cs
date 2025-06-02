using System.Collections.Generic;
using System.Threading.Tasks;
using UESAN.VDI.CORE.Core.Entities;

namespace UESAN.VDI.CORE.Core.Interfaces
{
    public interface IRevistasRepository
    {
        Task<List<Revistas>> GetAllActivasAsync();
        Task<List<Revistas>> GetAllAsync();
        Task<Revistas?> GetByIssnAsync(string issn, bool includeInactive = false);
        Task<string> CreateAsync(Revistas revista);
        Task<bool> UpdateAsync(Revistas revista);
        Task<bool> SoftDeleteAsync(string issn);
    }
}
