using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UESAN.VDI.CORE.Core.DTOs;
using UESAN.VDI.CORE.Core.Entities;
using UESAN.VDI.CORE.Core.Interfaces;

namespace UESAN.VDI.CORE.Core.Services
{
    public class SesionChatService : ISesionChatService
    {
        private readonly ISesionChatRepository _repository;
        public SesionChatService(ISesionChatRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<SesionChatListDTO>> GetAllAsync()
        {
            var sesiones = await _repository.GetAllAsync();
            return sesiones.Select(s => new SesionChatListDTO
            {
                SesionId = s.SesionId,
                FechaInicio = s.FechaInicio
            }).ToList();
        }

        public async Task<SesionChatDTO?> GetByIdAsync(int id)
        {
            var sesion = await _repository.GetByIdAsync(id);
            if (sesion == null) return null;
            return new SesionChatDTO
            {
                SesionId = sesion.SesionId,
                UsuarioId = sesion.UsuarioId,
                FechaInicio = sesion.FechaInicio,
                FechaFin = sesion.FechaFin
            };
        }

        public async Task<List<SesionChatListDTO>> GetByUsuarioIdAsync(int usuarioId)
        {
            var sesiones = await _repository.GetByUsuarioIdAsync(usuarioId);
            return sesiones.Select(s => new SesionChatListDTO
            {
                SesionId = s.SesionId,
                FechaInicio = s.FechaInicio
            }).ToList();
        }

        public async Task<int> CreateAsync(int usuarioId)
        {
            var sesion = new SesionChat
            {
                UsuarioId = usuarioId,
                // FechaInicio y FechaFin se autogeneran en la base de datos
            };
            return await _repository.CreateAsync(sesion);
        }
    }
}
