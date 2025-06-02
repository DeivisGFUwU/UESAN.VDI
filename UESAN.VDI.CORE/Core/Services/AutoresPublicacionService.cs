using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UESAN.VDI.CORE.Core.DTOs;
using UESAN.VDI.CORE.Core.Entities;
using UESAN.VDI.CORE.Core.Interfaces;

namespace UESAN.VDI.CORE.Core.Services
{
    public class AutoresPublicacionService : IAutoresPublicacionService
    {
        private readonly IAutoresPublicacionRepository _repository;
        public AutoresPublicacionService(IAutoresPublicacionRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<AutorPublicacionListDTO>> GetAllAsync()
        {
            var autores = await _repository.GetAllAsync();
            return autores.Select(a => new AutorPublicacionListDTO
            {
                Id = a.Id,
                OrdenAutor = a.OrdenAutor
            }).ToList();
        }

        public async Task<AutorPublicacionDTO?> GetByIdAsync(int id)
        {
            var autor = await _repository.GetByIdAsync(id);
            if (autor == null) return null;
            return new AutorPublicacionDTO
            {
                Id = autor.Id,
                PublicacionId = autor.PublicacionId,
                ProfesorId = autor.ProfesorId,
                OrdenAutor = autor.OrdenAutor,
                PorcentajeParticipacion = autor.PorcentajeParticipacion
            };
        }

        public async Task<int> CreateAsync(AutorPublicacionCreateDTO dto)
        {
            var entity = new AutoresPublicacion
            {
                PublicacionId = dto.PublicacionId,
                ProfesorId = dto.ProfesorId,
                OrdenAutor = dto.OrdenAutor,
                PorcentajeParticipacion = dto.PorcentajeParticipacion
            };
            return await _repository.CreateAsync(entity);
        }

        public async Task<bool> UpdateAsync(int id, AutorPublicacionDTO dto)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return false;
            entity.PublicacionId = dto.PublicacionId;
            entity.ProfesorId = dto.ProfesorId;
            entity.OrdenAutor = dto.OrdenAutor;
            entity.PorcentajeParticipacion = dto.PorcentajeParticipacion;
            return await _repository.UpdateAsync(entity);
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            return await _repository.SoftDeleteAsync(id);
        }
    }
}
