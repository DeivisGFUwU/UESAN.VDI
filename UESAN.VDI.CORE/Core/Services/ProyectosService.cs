using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UESAN.VDI.CORE.Core.DTOs;
using UESAN.VDI.CORE.Core.Entities;
using UESAN.VDI.CORE.Core.Interfaces;

namespace UESAN.VDI.CORE.Core.Services
{
    public class ProyectosService : IProyectosService
    {
        private readonly IProyectosRepository _proyectosRepository;
        public ProyectosService(IProyectosRepository proyectosRepository)
        {
            _proyectosRepository = proyectosRepository;
        }

        public async Task<List<ProyectoListDTO>> GetAllAsync()
        {
            var proyectos = await _proyectosRepository.GetAllAsync();
            return proyectos.Select(p => new ProyectoListDTO
            {
                ProyectoId = p.ProyectoId,
                Titulo = p.Titulo,
                Estatus = p.Estatus
            }).ToList();
        }

        public async Task<ProyectoDTO?> GetByIdAsync(int id)
        {
            var proyecto = await _proyectosRepository.GetByIdAsync(id);
            if (proyecto == null) return null;
            return new ProyectoDTO
            {
                ProyectoId = proyecto.ProyectoId,
                Titulo = proyecto.Titulo,
                Descripcion = proyecto.Descripcion,
                FechaInicio = proyecto.FechaInicio,
                FechaFin = proyecto.FechaFin,
                Estatus = proyecto.Estatus,
                Recomendado = proyecto.Recomendado,
                LineaId = proyecto.LineaId
            };
        }

        public async Task<int> CreateAsync(ProyectoDTO dto)
        {
            var proyecto = new Proyectos
            {
                Titulo = dto.Titulo,
                Descripcion = dto.Descripcion,
                FechaInicio = dto.FechaInicio,
                FechaFin = dto.FechaFin,
                Estatus = dto.Estatus,
                Recomendado = dto.Recomendado,
                LineaId = dto.LineaId
            };
            return await _proyectosRepository.CreateAsync(proyecto);
        }

        public async Task<bool> UpdateAsync(int id, ProyectoDTO dto)
        {
            var proyecto = await _proyectosRepository.GetByIdAsync(id);
            if (proyecto == null) return false;
            proyecto.Titulo = dto.Titulo;
            proyecto.Descripcion = dto.Descripcion;
            proyecto.FechaInicio = dto.FechaInicio;
            proyecto.FechaFin = dto.FechaFin;
            proyecto.Estatus = dto.Estatus;
            proyecto.Recomendado = dto.Recomendado;
            proyecto.LineaId = dto.LineaId;
            return await _proyectosRepository.UpdateAsync(proyecto);
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            var proyecto = await _proyectosRepository.GetByIdAsync(id);
            if (proyecto == null) return false;
            // Si hay un campo Activo/IsActive, usarlo aquí. Si no, eliminar físicamente o ignorar.
            // proyecto.Activo = false;
            // return await _proyectosRepository.UpdateAsync(proyecto);
            return false;
        }
    }
}
