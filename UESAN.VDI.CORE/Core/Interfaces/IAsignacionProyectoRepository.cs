using System.Collections.Generic;
using System.Threading.Tasks;
using UESAN.VDI.CORE.Core.Entities;

namespace UESAN.VDI.CORE.Core.Interfaces
{
    public interface IAsignacionProyectoRepository
    {
        Task<List<AsignacionProyecto>> GetAllAsync();
        Task<AsignacionProyecto?> GetByIdAsync(int id);
        Task<int> CreateAsync(AsignacionProyecto entity);
        Task<bool> UpdateAsync(AsignacionProyecto entity);
        Task<bool> SoftDeleteAsync(int id);
    }
}
