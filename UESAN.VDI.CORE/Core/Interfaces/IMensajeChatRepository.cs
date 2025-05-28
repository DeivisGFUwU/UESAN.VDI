using System.Collections.Generic;
using System.Threading.Tasks;
using UESAN.VDI.CORE.Core.Entities;

namespace UESAN.VDI.CORE.Core.Interfaces
{
    public interface IMensajeChatRepository
    {
        Task<List<MensajeChat>> GetBySesionIdAsync(int sesionId);
        Task<MensajeChat?> GetByIdAsync(int id);
        Task<int> CreateAsync(MensajeChat mensaje);
    }
}
