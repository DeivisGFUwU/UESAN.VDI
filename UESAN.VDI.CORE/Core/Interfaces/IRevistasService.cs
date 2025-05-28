using System.Collections.Generic;
using System.Threading.Tasks;
using UESAN.VDI.CORE.Core.DTOs;

namespace UESAN.VDI.CORE.Core.Interfaces
{
    public interface IRevistasService
    {
        Task<List<RevistaListDTO>> GetAllActivasAsync();
        Task<List<RevistaListDTO>> GetAllByRoleAsync(string? userRole);
        Task<RevistaDTO?> GetByIssnAsync(string issn, string? userRole = null);
        Task<string> CreateAsync(RevistaDTO dto);
        Task<bool> UpdateAsync(string issn, RevistaDTO dto);
        Task<bool> SoftDeleteAsync(string issn);
    }
}
