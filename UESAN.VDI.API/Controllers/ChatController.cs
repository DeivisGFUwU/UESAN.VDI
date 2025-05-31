using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UESAN.VDI.CORE.Core.DTOs;
using UESAN.VDI.CORE.Core.Interfaces;
using UESAN.VDI.CORE.Core.Helpers;
using System;

namespace UESAN.VDI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly ISesionChatService _sesionService;
        private readonly IMensajeChatService _mensajeService;
        public ChatController(ISesionChatService sesionService, IMensajeChatService mensajeService)
        {
            _sesionService = sesionService;
            _mensajeService = mensajeService;
        }

        [HttpGet("sesiones")]
        [RoleAuthorize(RoleHelper.ADMIN_ROLE, RoleHelper.PROFESOR_ROLE, RoleHelper.NORMAL_ROLE)]
        public async Task<IActionResult> GetSesiones()
        {
            var result = await _sesionService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("sesiones/usuario/{usuarioId}")]
        [RoleAuthorize(RoleHelper.ADMIN_ROLE, RoleHelper.PROFESOR_ROLE, RoleHelper.NORMAL_ROLE)]
        public async Task<IActionResult> GetSesionesPorUsuario(int usuarioId)
        {
            var result = await _sesionService.GetByUsuarioIdAsync(usuarioId);
            return Ok(result);
        }

        [HttpGet("sesiones/{id}")]
        [RoleAuthorize(RoleHelper.ADMIN_ROLE, RoleHelper.PROFESOR_ROLE, RoleHelper.NORMAL_ROLE)]
        public async Task<IActionResult> GetSesionById(int id)
        {
            var result = await _sesionService.GetByIdAsync(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpPost("sesiones")]
        [RoleAuthorize(RoleHelper.ADMIN_ROLE, RoleHelper.PROFESOR_ROLE, RoleHelper.NORMAL_ROLE)]
        public async Task<IActionResult> CreateSesion([FromBody] SesionChatCreateDTO dto)
        {
            // Asignar UsuarioId y FechaInicio desde el backend
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            var id = await _sesionService.CreateAsync(userId);
            return Ok(new { SesionId = id });
        }

        [HttpGet("mensajes/{sesionId}")]
        [RoleAuthorize(RoleHelper.ADMIN_ROLE, RoleHelper.PROFESOR_ROLE, RoleHelper.NORMAL_ROLE)]
        public async Task<IActionResult> GetMensajesPorSesion(int sesionId)
        {
            var result = await _mensajeService.GetBySesionIdAsync(sesionId);
            return Ok(result);
        }

        [HttpPost("mensajes")]
        [RoleAuthorize(RoleHelper.ADMIN_ROLE, RoleHelper.PROFESOR_ROLE, RoleHelper.NORMAL_ROLE)]
        public async Task<IActionResult> EnviarMensaje([FromBody] MensajeChatDTO dto)
        {
            // Asignar Remitente y FechaEnvio desde el backend
            dto.Remitente = User.Identity?.Name ?? "";
            dto.FechaEnvio = DateTime.UtcNow;
            var id = await _mensajeService.CreateAsync(dto);
            return Ok(new { MensajeId = id });
        }
    }
}
