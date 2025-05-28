using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UESAN.VDI.CORE.Core.Entities;
using UESAN.VDI.CORE.Core.Interfaces;
using UESAN.VDI.CORE.Infrastructure.data;

namespace UESAN.VDI.CORE.Infrastructure.Repositories
{
    public class FormulariosInvestigacionRepository : IFormulariosInvestigacionRepository
    {
        private readonly VdiDbContext _context;
        public FormulariosInvestigacionRepository(VdiDbContext context)
        {
            _context = context;
        }

        public async Task<List<FormulariosInvestigacion>> GetAllAsync()
        {
            return await _context.FormulariosInvestigacion.Where(f => f.Activo).ToListAsync();
        }

        public async Task<FormulariosInvestigacion?> GetByIdAsync(int id)
        {
            return await _context.FormulariosInvestigacion.FirstOrDefaultAsync(f => f.FormularioId == id && f.Activo);
        }

        public async Task<int> CreateAsync(FormulariosInvestigacion entity)
        {
            entity.Activo = true;
            _context.FormulariosInvestigacion.Add(entity);
            await _context.SaveChangesAsync();
            return entity.FormularioId;
        }

        public async Task<bool> UpdateAsync(FormulariosInvestigacion entity)
        {
            _context.FormulariosInvestigacion.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            var entity = await _context.FormulariosInvestigacion.FirstOrDefaultAsync(f => f.FormularioId == id && f.Activo);
            if (entity == null) return false;
            entity.Activo = false;
            _context.FormulariosInvestigacion.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
