using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UESAN.VDI.CORE.Core.Entities;
using UESAN.VDI.CORE.Core.Interfaces;
using UESAN.VDI.CORE.Infrastructure.data;

namespace UESAN.VDI.CORE.Infrastructure.Repositories
{
    public class RevistasRepository : IRevistasRepository
    {
        private readonly VdiDbContext _context;
        public RevistasRepository(VdiDbContext context)
        {
            _context = context;
        }

        public async Task<List<Revistas>> GetAllActivasAsync()
        {
            return await _context.Revistas.Where(r => r.Activa).ToListAsync();
        }

        public async Task<List<Revistas>> GetAllAsync()
        {
            return await _context.Revistas.Where(r => r.Activa).ToListAsync();
        }

        public async Task<Revistas?> GetByIssnAsync(string issn, bool includeInactive = false)
        {
            if (includeInactive)
                return await _context.Revistas.FirstOrDefaultAsync(r => r.Issn == issn);
            return await _context.Revistas.FirstOrDefaultAsync(r => r.Issn == issn && r.Activa);
        }

        public async Task<string> CreateAsync(Revistas revista)
        {
            revista.Activa = true;
            _context.Revistas.Add(revista);
            await _context.SaveChangesAsync();
            return revista.Issn;
        }

        public async Task<bool> UpdateAsync(Revistas revista)
        {
            _context.Revistas.Update(revista);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> SoftDeleteAsync(string issn)
        {
            var revista = await _context.Revistas.FirstOrDefaultAsync(r => r.Issn == issn && r.Activa);
            if (revista == null) return false;
            revista.Activa = false;
            _context.Revistas.Update(revista);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
