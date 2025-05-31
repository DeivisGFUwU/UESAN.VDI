using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UESAN.VDI.CORE.Core.Entities;
using UESAN.VDI.CORE.Core.Interfaces;
using UESAN.VDI.CORE.Infrastructure.data;

namespace UESAN.VDI.CORE.Infrastructure.Repositories
{
    public class UsuariosRepository : IUsuariosRepository
    {
        private readonly VdiDbContext _context;
        public UsuariosRepository(VdiDbContext context)
        {
            _context = context;
        }

        public async Task<Usuarios?> GetByCorreoAsync(string correo)
        {
            return await _context.Usuarios.Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Correo == correo && u.Activo);
        }

        public async Task<Usuarios?> GetByIdAsync(int id, bool includeInactive = false)
        {
            if (includeInactive)
                return await _context.Usuarios.Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.UsuarioId == id);
            return await _context.Usuarios.Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UsuarioId == id && u.Activo);
        }

        public async Task<List<Usuarios>> GetAllActivosAsync()
        {
            return await _context.Usuarios.Where(u => u.Activo).ToListAsync();
        }

        public async Task<List<Usuarios>> GetAllAsync()
        {
            return await _context.Usuarios.ToListAsync();
        }

        public async Task<bool> UpdateAsync(Usuarios usuario)
        {
            _context.Usuarios.Update(usuario);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> SoftDeleteAsync(int usuarioId)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.UsuarioId == usuarioId && u.Activo);
            if (usuario == null) return false;
            usuario.Activo = false;
            _context.Usuarios.Update(usuario);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<int> CreateAsync(Usuarios usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario.UsuarioId;
        }
    }
}
