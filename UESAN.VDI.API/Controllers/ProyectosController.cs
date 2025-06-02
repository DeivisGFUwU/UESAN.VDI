using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UESAN.VDI.CORE.Core.DTOs;
using UESAN.VDI.CORE.Core.Interfaces;
using UESAN.VDI.CORE.Core.Helpers;
using UESAN.VDI.CORE.Core.Entities;
using UESAN.VDI.CORE.Infrastructure.Repositories;

namespace UESAN.VDI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProyectosController : ControllerBase
    {
        private readonly IProyectosService _proyectosService;
        private readonly IProyectosRepository _proyectosRepository; // Fix: Change type from object to IProyectosRepository

        public ProyectosController(IProyectosService proyectosService, IProyectosRepository proyectosRepository) // Fix: Add IProyectosRepository to constructor
        {
            _proyectosService = proyectosService;
            _proyectosRepository = proyectosRepository; // Fix: Initialize _proyectosRepository
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
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)
                ?? User.FindFirst("sub")
                ?? User.FindFirst("id")
                ?? User.FindFirst("nameid")
                ?? User.FindFirst("UserId"); // <-- Aquí

            if (userIdClaim == null)
                return Unauthorized("No se pudo identificar al usuario.");

            int usuarioId = int.Parse(userIdClaim.Value);

            var proyecto = new Proyectos
            {
                Titulo = dto.Titulo,
                Descripcion = dto.Descripcion,
                FechaInicio = dto.FechaInicio,
                Estatus = dto.Estatus,
                Recomendado = dto.Recomendado,
                LineaId = dto.LineaId,
                AdminCrea = usuarioId
            };

            var id = await _proyectosRepository.CreateAsync(proyecto);
            return CreatedAtAction(nameof(GetById), new { id }, dto);
        }

        [HttpPut("{id}")]
        [RoleAuthorize(RoleHelper.ADMIN_ROLE, RoleHelper.PROFESOR_ROLE)]
        public async Task<IActionResult> Update(int id, [FromBody] ProyectoDTO dto)
        {
            var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
            if (!RoleHelper.IsProfesor(userRole))
            {
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
