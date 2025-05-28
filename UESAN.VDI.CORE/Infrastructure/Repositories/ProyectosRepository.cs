using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UESAN.VDI.CORE.Core.Entities;
using UESAN.VDI.CORE.Core.Interfaces;
using UESAN.VDI.CORE.Infrastructure.data;

namespace UESAN.VDI.CORE.Infrastructure.Repositories
{
    public class ProyectosRepository : IProyectosRepository
    {
        private readonly VdiDbContext _context;
        public ProyectosRepository(VdiDbContext context)
        {
            _context = context;
        }

        public async Task<List<Proyectos>> GetAllAsync()
        {
            return await _context.Proyectos.Where(p => p.Activo).ToListAsync();
        }

        public async Task<Proyectos?> GetByIdAsync(int id)
        {
            return await _context.Proyectos.FirstOrDefaultAsync(p => p.ProyectoId == id && p.Activo);
        }

        public async Task<int> CreateAsync(Proyectos proyecto)
        {
            proyecto.Activo = true;
            _context.Proyectos.Add(proyecto);
            await _context.SaveChangesAsync();
            return proyecto.ProyectoId;
        }

        public async Task<bool> UpdateAsync(Proyectos proyecto)
        {
            _context.Proyectos.Update(proyecto);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            var proyecto = await _context.Proyectos.FirstOrDefaultAsync(p => p.ProyectoId == id && p.Activo);
            if (proyecto == null) return false;
            proyecto.Activo = false;
            _context.Proyectos.Update(proyecto);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
