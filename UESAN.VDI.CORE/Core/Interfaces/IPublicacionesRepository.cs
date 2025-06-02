using Microsoft.EntityFrameworkCore;
using UESAN.VDI.CORE.Core.Entities;
using UESAN.VDI.CORE.Core.Interfaces;
using UESAN.VDI.CORE.Infrastructure.data;
using UESAN.VDI.CORE.Core.Interfaces;
using UESAN.VDI.CORE.Infrastructure.Repositories;

public class PublicacionesRepository : IPublicacionesRepository
{
    private readonly VdiDbContext _context;

    public PublicacionesRepository(VdiDbContext context)
    {
        _context = context;
    }

    public async Task<List<Publicaciones>> GetAllAsync()
    {
        return await _context.Publicaciones.ToListAsync();
    }

    public async Task<Publicaciones?> GetByIdAsync(int id)
    {
        return await _context.Publicaciones
            .Include(p => p.IssnNavigation)
            .FirstOrDefaultAsync(p => p.PublicacionId == id);
    }

    public async Task<int> CreateAsync(Publicaciones publicacion)
    {
        _context.Publicaciones.Add(publicacion);
        await _context.SaveChangesAsync();
        return publicacion.PublicacionId;
    }

    public async Task<bool> UpdateAsync(Publicaciones publicacion)
    {
        _context.Publicaciones.Update(publicacion);
        return await _context.SaveChangesAsync() > 0;
    }

    public Task<bool> SoftDeleteAsync(int id)
    {
        throw new NotImplementedException();
    }
}
public interface IPublicacionesRepository
{
    Task<List<Publicaciones>> GetAllAsync();
    Task<Publicaciones?> GetByIdAsync(int id);
    Task<int> CreateAsync(Publicaciones publicacion);
    Task<bool> UpdateAsync(Publicaciones publicacion);
    Task<bool> SoftDeleteAsync(int id);
}
