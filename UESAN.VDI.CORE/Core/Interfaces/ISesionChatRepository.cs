using System.Collections.Generic;
using System.Threading.Tasks;
using UESAN.VDI.CORE.Core.Entities;

namespace UESAN.VDI.CORE.Core.Interfaces
{
    public interface ISesionChatRepository
    {
        Task<List<SesionChat>> GetAllAsync();
        Task<SesionChat?> GetByIdAsync(int id);
        Task<List<SesionChat>> GetByUsuarioIdAsync(int usuarioId);
        Task<int> CreateAsync(SesionChat sesion);
    }
}
