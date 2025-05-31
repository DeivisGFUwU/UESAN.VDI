using System.Collections.Generic;
using System.Threading.Tasks;
using UESAN.VDI.CORE.Core.Entities;

namespace UESAN.VDI.CORE.Core.Interfaces
{
    public interface IUsuariosRepository
    {
        Task<Usuarios?> GetByCorreoAsync(string correo);
        Task<Usuarios?> GetByIdAsync(int id, bool includeInactive = false);
        Task<List<Usuarios>> GetAllActivosAsync();
        Task<List<Usuarios>> GetAllAsync(); // Para admin, incluye inactivos
        Task<bool> UpdateAsync(Usuarios usuario);
        Task<bool> SoftDeleteAsync(int usuarioId);
        Task<int> CreateAsync(Usuarios usuario);
    }
}
