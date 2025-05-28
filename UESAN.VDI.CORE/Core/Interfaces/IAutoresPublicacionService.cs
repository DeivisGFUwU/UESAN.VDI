using System.Collections.Generic;
using System.Threading.Tasks;
using UESAN.VDI.CORE.Core.DTOs;

namespace UESAN.VDI.CORE.Core.Interfaces
{
    public interface IAutoresPublicacionService
    {
        Task<List<AutorPublicacionListDTO>> GetAllAsync();
        Task<AutorPublicacionDTO?> GetByIdAsync(int id);
        Task<int> CreateAsync(AutorPublicacionDTO dto);
        Task<bool> UpdateAsync(int id, AutorPublicacionDTO dto);
    }
}
