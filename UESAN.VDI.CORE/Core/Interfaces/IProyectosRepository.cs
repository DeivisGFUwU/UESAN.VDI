using System.Collections.Generic;
using System.Threading.Tasks;
using UESAN.VDI.CORE.Core.Entities;

namespace UESAN.VDI.CORE.Core.Interfaces
{
    public interface IProyectosRepository
    {
        Task<List<Proyectos>> GetAllAsync();
        Task<Proyectos?> GetByIdAsync(int id);
        Task<int> CreateAsync(Proyectos proyecto);
        Task<bool> UpdateAsync(Proyectos proyecto);
    }
}
