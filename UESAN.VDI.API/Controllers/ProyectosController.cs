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
    public class ProyectosController : ControllerBase
    {
        private readonly IProyectosService _proyectosService;
        public ProyectosController(IProyectosService proyectosService)
        {
            _proyectosService = proyectosService;
        }

        [HttpGet]
        [RoleAuthorize(RoleHelper.ADMIN_ROLE, RoleHelper.PROFESOR_ROLE)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _proyectosService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        [RoleAuthorize(RoleHelper.ADMIN_ROLE, RoleHelper.PROFESOR_ROLE)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _proyectosService.GetByIdAsync(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpPost]
        [RoleAuthorize(RoleHelper.ADMIN_ROLE)]
        public async Task<IActionResult> Create([FromBody] ProyectoCreateDTO dto)
        {
            // Obtener el ID del usuario autenticado (admin)
            var adminCrea = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            var id = await _proyectosService.CreateAsync(dto, adminCrea);
            return CreatedAtAction(nameof(GetById), new { id }, dto);
        }

        [HttpPut("{id}")]
        [RoleAuthorize(RoleHelper.ADMIN_ROLE, RoleHelper.PROFESOR_ROLE)]
        public async Task<IActionResult> Update(int id, [FromBody] ProyectoDTO dto)
        {
            // Solo permitir que el profesor asigne FechaFin
            var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
            if (!RoleHelper.IsProfesor(userRole))
            {
                // Si no es profesor, no permitir modificar FechaFin
                dto.FechaFin = null;
            }
            var updated = await _proyectosService.UpdateAsync(id, dto);
            if (!updated)
                return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [RoleAuthorize(RoleHelper.ADMIN_ROLE)]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _proyectosService.SoftDeleteAsync(id);
            if (!deleted)
                return NotFound();
            return NoContent();
        }
    }
}
