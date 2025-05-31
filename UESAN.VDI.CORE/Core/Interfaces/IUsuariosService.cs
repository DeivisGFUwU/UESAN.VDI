using System.Collections.Generic;
using System.Threading.Tasks;
using UESAN.VDI.CORE.Core.DTOs;

namespace UESAN.VDI.CORE.Core.Interfaces
{
    public interface IUsuariosService
    {
        Task<UsuarioSignInResponseDTO?> SignInAsync(UsuarioSignInRequestDTO dto);
        Task<List<UsuarioListDTO>> GetAllActivosAsync();
        Task<UsuarioDTO?> GetByIdAsync(int id);
        // Nuevos métodos para control de roles y acceso
        Task<List<UsuarioListDTO>> GetAllAsync(string? userRole, bool includeInactive = false);
        Task<UsuarioDTO?> GetByIdAsync(int id, string? userRole);
        Task<bool> ReactivateAsync(int usuarioId);
        Task<bool> SoftDeleteAsync(int usuarioId);
        Task<int> CreateAsync(UsuarioCreateDTO dto);
    }
}
