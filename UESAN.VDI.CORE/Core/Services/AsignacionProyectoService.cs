using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UESAN.VDI.CORE.Core.DTOs;
using UESAN.VDI.CORE.Core.Entities;
using UESAN.VDI.CORE.Core.Interfaces;

namespace UESAN.VDI.CORE.Core.Services
{
    public class AsignacionProyectoService : IAsignacionProyectoService
    {
        private readonly IAsignacionProyectoRepository _repository;
        public AsignacionProyectoService(IAsignacionProyectoRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<AsignacionProyectoListDTO>> GetAllAsync()
        {
            var asignaciones = await _repository.GetAllAsync();
            return asignaciones.Select(a => new AsignacionProyectoListDTO
            {
                AsignacionId = a.AsignacionId,
                RolEnProyecto = a.RolEnProyecto
            }).ToList();
        }

        public async Task<AsignacionProyectoDTO?> GetByIdAsync(int id)
        {
            var asignacion = await _repository.GetByIdAsync(id);
            if (asignacion == null) return null;
            return new AsignacionProyectoDTO
            {
                AsignacionId = asignacion.AsignacionId,
                ProyectoId = asignacion.ProyectoId,
                ProfesorId = asignacion.ProfesorId,
                FechaAsignacion = asignacion.FechaAsignacion,
                RolEnProyecto = asignacion.RolEnProyecto
            };
        }

        public async Task<int> CreateAsync(AsignacionProyectoCreateDTO dto)
        {
            var entity = new AsignacionProyecto
            {
                ProyectoId = dto.ProyectoId,
                ProfesorId = dto.ProfesorId,
                // FechaAsignacion se autogenera en la base de datos
                RolEnProyecto = dto.RolEnProyecto,
                Activo = true
            };
            return await _repository.CreateAsync(entity);
        }

        public async Task<bool> UpdateAsync(int id, AsignacionProyectoDTO dto)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return false;
            entity.ProyectoId = dto.ProyectoId;
            entity.ProfesorId = dto.ProfesorId;
            entity.FechaAsignacion = dto.FechaAsignacion;
            entity.RolEnProyecto = dto.RolEnProyecto;
            return await _repository.UpdateAsync(entity);
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            return await _repository.SoftDeleteAsync(id);
        }
    }
}
