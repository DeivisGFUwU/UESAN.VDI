using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UESAN.VDI.CORE.Core.DTOs;
using UESAN.VDI.CORE.Core.Interfaces;
using UESAN.VDI.CORE.Core.Helpers;

namespace UESAN.VDI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuariosService _usuariosService;
        public UsuariosController(IUsuariosService usuariosService)
        {
            _usuariosService = usuariosService;
        }

        [HttpPost("signIn")]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn([FromBody] UsuarioSignInRequestDTO dto)
        {
            var result = await _usuariosService.SignInAsync(dto);
            if (result == null)
                return Unauthorized();
            return Ok(result);
        }

        [HttpPost]
        [RoleAuthorize(RoleHelper.ADMIN_ROLE)]
        public async Task<IActionResult> Create([FromBody] UsuarioCreateDTO dto)
        {
            var id = await _usuariosService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id }, dto);
        }

        [HttpPost("registrar")]
        [AllowAnonymous]
        public async Task<IActionResult> Registrar([FromBody] UsuarioCreateDTO dto)
        {
            // Forzar el rol a usuario com�n (por ejemplo, RoleId = 3)
            dto.RoleId = 3;
            var id = await _usuariosService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id }, dto);
        }

        [HttpGet]
        [RoleAuthorize(RoleHelper.ADMIN_ROLE)]
        public async Task<IActionResult> GetAllActivos()
        {
            var result = await _usuariosService.GetAllActivosAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        [RoleAuthorize(RoleHelper.ADMIN_ROLE, RoleHelper.PROFESOR_ROLE)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _usuariosService.GetByIdAsync(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpPost("reactivar/{id}")]
        [RoleAuthorize(RoleHelper.ADMIN_ROLE)]
        public async Task<IActionResult> Reactivate(int id)
        {
            var reactivated = await _usuariosService.ReactivateAsync(id);
            if (!reactivated)
                return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [RoleAuthorize(RoleHelper.ADMIN_ROLE)]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _usuariosService.SoftDeleteAsync(id);
            if (!deleted)
                return NotFound();
            return NoContent();
        }

        [HttpPut("cambiar-clave")]
        public async Task<IActionResult> CambiarClave([FromBody] CambiarClaveDTO dto)
        {
            var usuarioId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            var (success, errorMessage) = await _usuariosService.CambiarClaveAsync(usuarioId, dto);
            
            if (!success)
                return BadRequest(errorMessage);
                
            return Ok("Contrase�a actualizada correctamente");
        }

        [HttpPost("recuperar-clave")]
        [AllowAnonymous]
        public async Task<IActionResult> RecuperarClave([FromBody] RecuperarClaveDTO dto)
        {
            await _usuariosService.RecuperarClaveAsync(dto.Correo);
            return Ok("Si el correo existe, se han enviado instrucciones para recuperar la contrase�a.");
        }
    }
}
