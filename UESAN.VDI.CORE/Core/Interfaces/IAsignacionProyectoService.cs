using System.Collections.Generic;
using System.Threading.Tasks;
using UESAN.VDI.CORE.Core.DTOs;

namespace UESAN.VDI.CORE.Core.Interfaces
{
    public interface IAsignacionProyectoService
    {
        Task<List<AsignacionProyectoListDTO>> GetAllAsync();
        Task<AsignacionProyectoDTO?> GetByIdAsync(int id);
        Task<int> CreateAsync(AsignacionProyectoCreateDTO dto);
        Task<bool> UpdateAsync(int id, AsignacionProyectoDTO dto);
        Task<bool> SoftDeleteAsync(int id);
    }
}
