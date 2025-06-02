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
        private readonly IUsuariosRepository _usuariosRepository;

        public ProyectosService(IProyectosRepository proyectosRepository, IUsuariosRepository usuariosRepository)
        {
            _proyectosRepository = proyectosRepository;
            _usuariosRepository = usuariosRepository;
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

        public async Task<int> CreateAsync(ProyectoCreateDTO dto, int adminCrea)
        {
            // Validar que el usuario adminCrea existe y está activo
            var usuario = await _usuariosRepository.GetByIdAsync(adminCrea);
            if (usuario == null)
                throw new System.Exception($"No existe un usuario activo con el id {adminCrea}");

            var proyecto = new Proyectos
            {
                Titulo = dto.Titulo,
                Descripcion = dto.Descripcion,
                FechaInicio = dto.FechaInicio,
                Estatus = dto.Estatus,
                Recomendado = dto.Recomendado,
                LineaId = dto.LineaId,
                AdminCrea = adminCrea
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
            // Solo permitir modificar FechaFin si el DTO la trae y el usuario es profesor
            // (La lógica de rol debe implementarse en el controlador, aquí solo se respeta el valor)
            if (dto.FechaFin != null)
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
            proyecto.Activo = false;
            return await _proyectosRepository.UpdateAsync(proyecto);
        }
    }
}
