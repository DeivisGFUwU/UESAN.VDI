using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UESAN.VDI.CORE.Core.DTOs;
using UESAN.VDI.CORE.Core.Entities;
using UESAN.VDI.CORE.Core.Interfaces;

namespace UESAN.VDI.CORE.Core.Services
{
    public class LineasInvestigacionService : ILineasInvestigacionService
    {
        private readonly ILineasInvestigacionRepository _lineasInvestigacionRepository;
        public LineasInvestigacionService(ILineasInvestigacionRepository lineasInvestigacionRepository)
        {
            _lineasInvestigacionRepository = lineasInvestigacionRepository;
        }
        //Get all LineasInvestigacion
        public async Task<List<LineasInvestigacionDTO>> GetlineasInvestigacion()
        {
            var lineasInvestigacion = await _lineasInvestigacionRepository.GetAllLineasInvestigacion();
            return lineasInvestigacion.Select(li => new LineasInvestigacionDTO
            {
                LineaId = li.LineaId,
                Nombre = li.Nombre
            }).ToList();
        }

        //Get LineaInvestigacion by Id
        public async Task<LineasInvestigacionDTO?> GetLineasInvestigacionById(int id)
        {
            var lineaInvestigacion = await _lineasInvestigacionRepository.GetLineasInvestigacionById(id);
            if (lineaInvestigacion == null)
            {
                return null;
            }
            return new LineasInvestigacionDTO
            {
                LineaId = lineaInvestigacion.LineaId,
                Nombre = lineaInvestigacion.Nombre
            };
        }

        //Create LineaInvestigacion
        public async Task<int> CreateLineasInvestigacion(LineasInvestigacionCreateDTO lineaInvestigacionDto)
        {
            var lineaInvestigacion = new LineasInvestigacion
            {
                Nombre = lineaInvestigacionDto.Nombre
            };
            var id = await _lineasInvestigacionRepository.CreateLineasInvestigacion(lineaInvestigacion);
            return id;
        }

        // Update LineaInvestigacion
        public async Task<bool> UpdateLineasInvestigacion(LineasInvestigacionListDTO lineasInvestigacionDto)
        {
            var lineasInvestigacion = new LineasInvestigacion()
            {
                LineaId = lineasInvestigacionDto.LineaId,
                Nombre = lineasInvestigacionDto.Nombre
            };
            var result = await _lineasInvestigacionRepository.UpdateLineasInvestigacion(lineasInvestigacion);
            return result;
        }

        // Delete LineaInvestigacion
        public async Task DeleteLineasInvestigacion(int id)
        {
            await _lineasInvestigacionRepository.DeleteLineasInvestigacion(id);
        }
    }
}
