using System.Collections.Generic;
using System.Threading.Tasks;
using UESAN.VDI.CORE.Core.Entities;

namespace UESAN.VDI.CORE.Core.Interfaces
{
    public interface IProfesoresRepository
    {
        Task<List<Profesores>> GetAllActivosAsync();
        Task<Profesores?> GetByIdAsync(int id);
        Task<Profesores?> GetByUsuarioIdAsync(int usuarioId);
        Task<int> CreateAsync(Profesores profesor);
        Task<bool> UpdateAsync(Profesores profesor);
    }
}
