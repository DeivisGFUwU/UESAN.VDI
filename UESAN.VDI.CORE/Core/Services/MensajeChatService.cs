using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UESAN.VDI.CORE.Core.DTOs;
using UESAN.VDI.CORE.Core.Entities;
using UESAN.VDI.CORE.Core.Interfaces;

namespace UESAN.VDI.CORE.Core.Services
{
    public class MensajeChatService : IMensajeChatService
    {
        private readonly IMensajeChatRepository _repository;
        public MensajeChatService(IMensajeChatRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<MensajeChatListDTO>> GetBySesionIdAsync(int sesionId)
        {
            var mensajes = await _repository.GetBySesionIdAsync(sesionId);
            return mensajes.Select(m => new MensajeChatListDTO
            {
                MensajeId = m.MensajeId,
                Remitente = m.Remitente,
                FechaEnvio = m.FechaEnvio
            }).ToList();
        }

        public async Task<MensajeChatDTO?> GetByIdAsync(int id)
        {
            var mensaje = await _repository.GetByIdAsync(id);
            if (mensaje == null) return null;
            return new MensajeChatDTO
            {
                MensajeId = mensaje.MensajeId,
                SesionId = mensaje.SesionId,
                Remitente = mensaje.Remitente,
                Texto = mensaje.Texto,
                FechaEnvio = mensaje.FechaEnvio
            };
        }

        public async Task<int> CreateAsync(MensajeChatDTO dto)
        {
            var mensaje = new MensajeChat
            {
                SesionId = dto.SesionId,
                Remitente = dto.Remitente,
                Texto = dto.Texto,
                FechaEnvio = dto.FechaEnvio
            };
            return await _repository.CreateAsync(mensaje);
        }
    }
}
