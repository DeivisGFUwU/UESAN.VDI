using System.Collections.Generic;
using System.Threading.Tasks;
using UESAN.VDI.CORE.Core.DTOs;

namespace UESAN.VDI.CORE.Core.Interfaces
{
    public interface IPublicacionesService
    {
        Task<List<PublicacionListDTO>> GetAllAsync();
        Task<PublicacionDTO?> GetByIdAsync(int id);
        Task<int> CreateAsync(PublicacionCreateDTO dto);
        Task<bool> UpdateAsync(int id, PublicacionDTO dto);
        Task<bool> SoftDeleteAsync(int id);
        Task<List<PublicacionListDTO>> GetAllFilteredAsync(string? userRole, string? userId, int? profesorId, string? issn, DateTime? fechaInicio, DateTime? fechaFin);
        Task<PublicacionDTO?> GetByIdFilteredAsync(int id, string? userRole, string? userId);
        Task<int> CreateValidatedAsync(PublicacionCreateDTO dto, string? userRole, string? userId);
        Task<bool> UpdateValidatedAsync(int id, PublicacionDTO dto, string? userRole, string? userId);
        Task<bool> SoftDeleteValidatedAsync(int id, string? userRole, string? userId);
    }
}
