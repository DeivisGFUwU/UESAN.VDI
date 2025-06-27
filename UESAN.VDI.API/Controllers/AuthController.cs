using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UESAN.VDI.CORE.Core.DTOs;
using UESAN.VDI.CORE.Core.Interfaces;
using UESAN.VDI.CORE.Core.Helpers;
using OfficeOpenXml;
using System.Collections.Generic;
using System;

namespace UESAN.VDI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUsuariosService _usuariosService;
        private readonly IEmailService _emailService;
        public AuthController(IUsuariosService usuariosService, IEmailService emailService)
        {
            _usuariosService = usuariosService;
            _emailService = emailService;
        }

        // POST: api/Auth/enviar-contraseña
        [HttpPost("enviar-contraseña")]
        [AllowAnonymous]
        public async Task<IActionResult> EnviarContrasena([FromBody] EnviarContrasenaRequest request)
        {
            var usuario = await _usuariosService.GetByCorreoAsync(request.Correo);
            if (usuario == null)
                return NotFound("No existe un usuario con ese correo.");
            if (string.IsNullOrEmpty(usuario.Password))
                return BadRequest("No hay contraseña registrada para este usuario.");
            await _emailService.SendEmailAsync(request.Correo, "Recuperación de contraseña", $"Su contraseña es: {usuario.Password}");
            return Ok("La contraseña ha sido enviada al correo.");
        }
    }

    public class EnviarContrasenaRequest
    {
        public string Correo { get; set; } = string.Empty;
    }
}
