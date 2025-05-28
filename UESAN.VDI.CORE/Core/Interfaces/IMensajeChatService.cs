using System.Collections.Generic;
using System.Threading.Tasks;
using UESAN.VDI.CORE.Core.DTOs;

namespace UESAN.VDI.CORE.Core.Interfaces
{
    public interface IMensajeChatService
    {
        Task<List<MensajeChatListDTO>> GetBySesionIdAsync(int sesionId);
        Task<MensajeChatDTO?> GetByIdAsync(int id);
        Task<int> CreateAsync(MensajeChatDTO dto);
    }
}
