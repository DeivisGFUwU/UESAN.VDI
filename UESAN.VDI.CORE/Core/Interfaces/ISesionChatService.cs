using System.Collections.Generic;
using System.Threading.Tasks;
using UESAN.VDI.CORE.Core.DTOs;

namespace UESAN.VDI.CORE.Core.Interfaces
{
    public interface ISesionChatService
    {
        Task<List<SesionChatListDTO>> GetAllAsync();
        Task<SesionChatDTO?> GetByIdAsync(int id);
        Task<List<SesionChatListDTO>> GetByUsuarioIdAsync(int usuarioId);
        Task<int> CreateAsync(int usuarioId);
    }
}
