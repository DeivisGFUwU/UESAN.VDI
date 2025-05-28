using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UESAN.VDI.CORE.Core.Entities;
using UESAN.VDI.CORE.Core.Interfaces;
using UESAN.VDI.CORE.Infrastructure.data;

namespace UESAN.VDI.CORE.Infrastructure.Repositories
{
    public class SesionChatRepository : ISesionChatRepository
    {
        private readonly VdiDbContext _context;
        public SesionChatRepository(VdiDbContext context)
        {
            _context = context;
        }

        public async Task<List<SesionChat>> GetAllAsync()
        {
            return await _context.SesionChat.Where(s => s.Activo).ToListAsync();
        }

        public async Task<SesionChat?> GetByIdAsync(int id)
        {
            return await _context.SesionChat.FirstOrDefaultAsync(s => s.SesionId == id && s.Activo);
        }

        public async Task<List<SesionChat>> GetByUsuarioIdAsync(int usuarioId)
        {
            return await _context.SesionChat.Where(s => s.UsuarioId == usuarioId && s.Activo).ToListAsync();
        }

        public async Task<int> CreateAsync(SesionChat sesion)
        {
            sesion.Activo = true;
            await _context.SesionChat.AddAsync(sesion);
            await _context.SaveChangesAsync();
            return sesion.SesionId;
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            var sesion = await _context.SesionChat.FirstOrDefaultAsync(s => s.SesionId == id && s.Activo);
            if (sesion == null) return false;
            sesion.Activo = false;
            _context.SesionChat.Update(sesion);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
