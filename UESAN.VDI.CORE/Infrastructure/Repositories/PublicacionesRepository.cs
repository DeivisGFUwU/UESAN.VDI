using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UESAN.VDI.CORE.Core.Entities;
using UESAN.VDI.CORE.Core.Interfaces;
using UESAN.VDI.CORE.Infrastructure.data;

namespace UESAN.VDI.CORE.Infrastructure.Repositories
{
    public class PublicacionesRepository : IPublicacionesRepository
    {
        private readonly VdiDbContext _context;
        public PublicacionesRepository(VdiDbContext context)
        {
            _context = context;
        }

        public async Task<List<Publicaciones>> GetAllAsync()
        {
            return await _context.Publicaciones
                .Include(p => p.IssnNavigation)
                .Where(p => p.Activo)
                .ToListAsync();
        }

        public async Task<Publicaciones?> GetByIdAsync(int id)
        {
            return await _context.Publicaciones.FirstOrDefaultAsync(p => p.PublicacionId == id && p.Activo);
        }

        public async Task<int> CreateAsync(Publicaciones publicacion)
        {
            publicacion.Activo = true;
            _context.Publicaciones.Add(publicacion);
            await _context.SaveChangesAsync();
            return publicacion.PublicacionId;
        }

        public async Task<bool> UpdateAsync(Publicaciones publicacion)
        {
            _context.Publicaciones.Update(publicacion);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            var publicacion = await _context.Publicaciones.FirstOrDefaultAsync(p => p.PublicacionId == id && p.Activo);
            if (publicacion == null) return false;
            publicacion.Activo = false;
            _context.Publicaciones.Update(publicacion);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
