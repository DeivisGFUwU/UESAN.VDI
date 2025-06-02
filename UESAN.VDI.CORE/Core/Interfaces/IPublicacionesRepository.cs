using System.Collections.Generic;
using System.Threading.Tasks;
using UESAN.VDI.CORE.Core.Entities;

namespace UESAN.VDI.CORE.Core.Interfaces
{
    public interface IPublicacionesRepository
    {
        Task<List<Publicaciones>> GetAllAsync();
        Task<Publicaciones?> GetByIdAsync(int id);
        Task<int> CreateAsync(Publicaciones publicacion);
        Task<bool> UpdateAsync(Publicaciones publicacion);
        Task<bool> SoftDeleteAsync(int id);
    }
}
