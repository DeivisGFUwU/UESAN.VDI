using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UESAN.VDI.CORE.Core.Entities;
using UESAN.VDI.CORE.Core.Interfaces;
using UESAN.VDI.CORE.Infrastructure.data;

namespace UESAN.VDI.CORE.Infrastructure.Repositories
{
    public class MensajeChatRepository : IMensajeChatRepository
    {
        private readonly VdiDbContext _context;
        public MensajeChatRepository(VdiDbContext context)
        {
            _context = context;
        }

        public async Task<List<MensajeChat>> GetBySesionIdAsync(int sesionId)
        {
            return await _context.MensajeChat.Where(m => m.SesionId == sesionId && m.Activo).ToListAsync();
        }

        public async Task<MensajeChat?> GetByIdAsync(int id)
        {
            return await _context.MensajeChat.FirstOrDefaultAsync(m => m.MensajeId == id && m.Activo);
        }

        public async Task<int> CreateAsync(MensajeChat mensaje)
        {
            mensaje.Activo = true;
            await _context.MensajeChat.AddAsync(mensaje);
            await _context.SaveChangesAsync();
            return mensaje.MensajeId;
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            var mensaje = await _context.MensajeChat.FirstOrDefaultAsync(m => m.MensajeId == id && m.Activo);
            if (mensaje == null) return false;
            mensaje.Activo = false;
            _context.MensajeChat.Update(mensaje);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
