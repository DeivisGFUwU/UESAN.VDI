using System.Collections.Generic;
using System.Threading.Tasks;
using UESAN.VDI.CORE.Core.DTOs;

namespace UESAN.VDI.CORE.Core.Interfaces
{
    public interface IProyectosService
    {
        Task<List<ProyectoListDTO>> GetAllAsync();
        Task<ProyectoDTO?> GetByIdAsync(int id);
        Task<int> CreateAsync(ProyectoCreateDTO dto, int adminCrea);
        Task<bool> UpdateAsync(int id, ProyectoDTO dto);
        Task<bool> SoftDeleteAsync(int id);
    }
}
