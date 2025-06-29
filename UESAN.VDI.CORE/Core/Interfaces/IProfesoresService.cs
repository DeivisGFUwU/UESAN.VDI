using System.Collections.Generic;
using System.Threading.Tasks;
using UESAN.VDI.CORE.Core.DTOs;

namespace UESAN.VDI.CORE.Core.Interfaces
{
    public interface IProfesoresService
    {
        Task<List<ProfesorListDTO>> GetAllActivosAsync();
        Task<ProfesorDTO?> GetByIdAsync(int id);
        Task<int> CreateAsync(ProfesorCreateDTO dto);
        Task<bool> UpdateAsync(int id, ProfesorDTO dto);
        Task<bool> SoftDeleteAsync(int id);
        Task<bool> ReactivateAsync(int id);
        Task<int> CrearMasivoAsync(List<ProfesorCreateDTO> profesores);
        Task<bool> CrearProfesorConUsuarioAsync(ProfesorUsuarioCreateDTO dto);
        Task<ProfesoresMasivoResultadoDTO> CrearProfesoresConUsuariosMasivoAsync(List<ProfesorUsuarioCreateDTO> dtos);
        Task<(bool Success, string? ErrorMessage)> CrearProfesorConUsuarioDebugAsync(ProfesorUsuarioCreateDTO dto);
    }
}
