using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UESAN.VDI.CORE.Core.DTOs;
using UESAN.VDI.CORE.Core.Interfaces;
using UESAN.VDI.CORE.Core.Helpers;

namespace UESAN.VDI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PublicacionesController : ControllerBase
    {
        private readonly IPublicacionesService _publicacionesService;
        public PublicacionesController(IPublicacionesService publicacionesService)
        {
            _publicacionesService = publicacionesService;
        }

        [HttpGet]
        [RoleAuthorize(RoleHelper.ADMIN_ROLE, RoleHelper.PROFESOR_ROLE, RoleHelper.NORMAL_ROLE)]
        public async Task<IActionResult> GetAll([FromQuery] int? profesorId, [FromQuery] string? issn, [FromQuery] DateTime? fechaInicio, [FromQuery] DateTime? fechaFin)
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _publicacionesService.GetAllFilteredAsync(userRole, userId, profesorId, issn, fechaInicio, fechaFin);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [RoleAuthorize(RoleHelper.ADMIN_ROLE, RoleHelper.PROFESOR_ROLE, RoleHelper.NORMAL_ROLE)]
        public async Task<IActionResult> GetById(int id)
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _publicacionesService.GetByIdFilteredAsync(id, userRole, userId);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpPost]
        [RoleAuthorize(RoleHelper.ADMIN_ROLE, RoleHelper.PROFESOR_ROLE)]
        public async Task<IActionResult> Create([FromBody] PublicacionCreateDTO dto)
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var id = await _publicacionesService.CreateValidatedAsync(dto, userRole, userId);
            if (id == -1)
                return BadRequest("Profesor o Revista no existen, o no tiene permisos.");
            return CreatedAtAction(nameof(GetById), new { id }, dto);
        }

        [HttpPut("{id}")]
        [RoleAuthorize(RoleHelper.ADMIN_ROLE, RoleHelper.PROFESOR_ROLE)]
        public async Task<IActionResult> Update(int id, [FromBody] PublicacionDTO dto)
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var updated = await _publicacionesService.UpdateValidatedAsync(id, dto, userRole, userId);
            if (!updated)
                return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [RoleAuthorize(RoleHelper.ADMIN_ROLE, RoleHelper.PROFESOR_ROLE)]
        public async Task<IActionResult> Delete(int id)
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var deleted = await _publicacionesService.SoftDeleteValidatedAsync(id, userRole, userId);
            if (!deleted)
                return NotFound();
            return NoContent();
        }
    }
}
