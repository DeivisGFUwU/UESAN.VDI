using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UESAN.VDI.CORE.Core.Entities;
using UESAN.VDI.CORE.Core.Interfaces;
using UESAN.VDI.CORE.Infrastructure.data;

namespace UESAN.VDI.CORE.Infrastructure.Repositories
{
    public class AsignacionProyectoRepository : IAsignacionProyectoRepository
    {
        private readonly VdiDbContext _context;
        public AsignacionProyectoRepository(VdiDbContext context)
        {
            _context = context;
        }

        public async Task<List<AsignacionProyecto>> GetAllAsync()
        {
            return await _context.AsignacionProyecto.Where(a => a.Activo).ToListAsync();
        }

        public async Task<AsignacionProyecto?> GetByIdAsync(int id)
        {
            return await _context.AsignacionProyecto.FirstOrDefaultAsync(a => a.AsignacionId == id && a.Activo);
        }

        public async Task<int> CreateAsync(AsignacionProyecto entity)
        {
            entity.Activo = true;
            _context.AsignacionProyecto.Add(entity);
            await _context.SaveChangesAsync();
            return entity.AsignacionId;
        }

        public async Task<bool> UpdateAsync(AsignacionProyecto entity)
        {
            _context.AsignacionProyecto.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            var entity = await _context.AsignacionProyecto.FirstOrDefaultAsync(a => a.AsignacionId == id && a.Activo);
            if (entity == null) return false;
            entity.Activo = false;
            _context.AsignacionProyecto.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
