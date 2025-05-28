using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UESAN.VDI.CORE.Core.Entities;
using UESAN.VDI.CORE.Core.Interfaces;
using UESAN.VDI.CORE.Infrastructure.data;

namespace UESAN.VDI.CORE.Infrastructure.Repositories
{
    public class AutoresPublicacionRepository : IAutoresPublicacionRepository
    {
        private readonly VdiDbContext _context;
        public AutoresPublicacionRepository(VdiDbContext context)
        {
            _context = context;
        }

        public async Task<List<AutoresPublicacion>> GetAllAsync()
        {
            return await _context.AutoresPublicacion.Where(a => a.Activo).ToListAsync();
        }

        public async Task<AutoresPublicacion?> GetByIdAsync(int id)
        {
            return await _context.AutoresPublicacion.FirstOrDefaultAsync(a => a.Id == id && a.Activo);
        }

        public async Task<int> CreateAsync(AutoresPublicacion entity)
        {
            entity.Activo = true;
            _context.AutoresPublicacion.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<bool> UpdateAsync(AutoresPublicacion entity)
        {
            _context.AutoresPublicacion.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            var entity = await _context.AutoresPublicacion.FirstOrDefaultAsync(a => a.Id == id && a.Activo);
            if (entity == null) return false;
            entity.Activo = false;
            _context.AutoresPublicacion.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
