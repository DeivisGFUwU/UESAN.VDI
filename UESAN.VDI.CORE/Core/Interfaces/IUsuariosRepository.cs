using System.Collections.Generic;
using System.Threading.Tasks;
using UESAN.VDI.CORE.Core.Entities;

namespace UESAN.VDI.CORE.Core.Interfaces
{
    public interface IUsuariosRepository
    {
        Task<Usuarios?> GetByCorreoAsync(string correo);
        Task<Usuarios?> GetByIdAsync(int id);
        Task<List<Usuarios>> GetAllActivosAsync();
        Task<List<Usuarios>> GetAllAsync(); // Para admin, incluye inactivos
    }
}
