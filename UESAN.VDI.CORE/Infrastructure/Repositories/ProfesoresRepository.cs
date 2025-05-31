using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UESAN.VDI.CORE.Core.Entities;
using UESAN.VDI.CORE.Core.Interfaces;
using UESAN.VDI.CORE.Infrastructure.data;

namespace UESAN.VDI.CORE.Infrastructure.Repositories
{
    public class ProfesoresRepository : IProfesoresRepository
    {
        private readonly VdiDbContext _context;
        public ProfesoresRepository(VdiDbContext context)
        {
            _context = context;
        }

        public async Task<List<Profesores>> GetAllActivosAsync()
        {
            return await _context.Profesores.Where(p => p.Activo).ToListAsync();
        }

        public async Task<Profesores?> GetByIdAsync(int id)
        {
            return await _context.Profesores.FirstOrDefaultAsync(p => p.ProfesorId == id && p.Activo);
        }

        public async Task<Profesores?> GetByUsuarioIdAsync(int usuarioId)
        {
            return await _context.Profesores.FirstOrDefaultAsync(p => p.UsuarioId == usuarioId);
        }

        public async Task<int> CreateAsync(Profesores profesor)
        {
            _context.Profesores.Add(profesor);
            await _context.SaveChangesAsync();
            return profesor.ProfesorId;
        }

        public async Task<bool> UpdateAsync(Profesores profesor)
        {
            _context.Profesores.Update(profesor);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
