using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UESAN.VDI.CORE.Core.Entities;
using UESAN.VDI.CORE.Core.Interfaces;
using UESAN.VDI.CORE.Infrastructure.data;

namespace UESAN.VDI.CORE.Infrastructure.Repositories
{
    public class LineasInvestigacionRepository : ILineasInvestigacionRepository
    {
        private readonly VdiDbContext _context;
        public LineasInvestigacionRepository(VdiDbContext context)
        {
            _context = context;
        }
        public async Task<List<LineasInvestigacion>> GetAllLineasInvestigacion()
        {
            return await _context.LineasInvestigacion.Where(l => l.Activo).ToListAsync();
        }
        public async Task<LineasInvestigacion> GetLineasInvestigacionById(int id)
        {
            return await _context.LineasInvestigacion.FirstOrDefaultAsync(l => l.LineaId == id && l.Activo);
        }
        public async Task<int> CreateLineasInvestigacion(LineasInvestigacion lineaInvestigacion)
        {
            lineaInvestigacion.Activo = true;
            await _context.LineasInvestigacion.AddAsync(lineaInvestigacion);
            await _context.SaveChangesAsync();
            return lineaInvestigacion.LineaId;
        }

        public async Task<bool> UpdateLineasInvestigacion(LineasInvestigacion lineaInvestigacion)
        {
            bool result = false;
            var existingLineasInvestigacion = await _context.LineasInvestigacion.FindAsync(lineaInvestigacion.LineaId);
            if (existingLineasInvestigacion != null && existingLineasInvestigacion.Activo)
            {
                existingLineasInvestigacion.Nombre = lineaInvestigacion.Nombre;
                // Update other properties as needed
                _context.LineasInvestigacion.Update(existingLineasInvestigacion);
                await _context.SaveChangesAsync();
                result = true;
            }
            return result;
        }

        public async Task DeleteLineasInvestigacion(int id)
        {
            var lineaInvestigacion = await _context.LineasInvestigacion.FirstOrDefaultAsync(l => l.LineaId == id && l.Activo);
            if (lineaInvestigacion != null)
            {
                lineaInvestigacion.Activo = false;
                _context.LineasInvestigacion.Update(lineaInvestigacion);
                await _context.SaveChangesAsync();
            }
        }
    }
}
