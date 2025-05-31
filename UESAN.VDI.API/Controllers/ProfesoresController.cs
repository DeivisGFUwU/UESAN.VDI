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
    public class ProfesoresController : ControllerBase
    {
        private readonly IProfesoresService _profesoresService;
        public ProfesoresController(IProfesoresService profesoresService)
        {
            _profesoresService = profesoresService;
        }

        // Get all (/Profesores)
        [HttpGet]
        [RoleAuthorize(RoleHelper.ADMIN_ROLE)]
        public async Task<IActionResult> GetAllActivos()
        {
            var result = await _profesoresService.GetAllActivosAsync();
            return Ok(result);
        }

        // Get all (/Profesores/2)
        [HttpGet("{id}")]
        [RoleAuthorize(RoleHelper.ADMIN_ROLE, RoleHelper.PROFESOR_ROLE)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _profesoresService.GetByIdAsync(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        // Post (/Profesores)
        [HttpPost]
        [RoleAuthorize(RoleHelper.ADMIN_ROLE)]
        public async Task<IActionResult> Create([FromBody] ProfesorCreateDTO dto)
        {
            var id = await _profesoresService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id }, dto);
        }

        // Put (/Profesores/5)
        [HttpPut("{id}")]
        [RoleAuthorize(RoleHelper.ADMIN_ROLE)]
        public async Task<IActionResult> Update(int id, [FromBody] ProfesorDTO dto)
        {
            var updated = await _profesoresService.UpdateAsync(id, dto);
            if (!updated)
                return NotFound();
            return NoContent();
        }

        // Soft Delete (/Profesores/5)
        [HttpDelete("{id}")]
        [RoleAuthorize(RoleHelper.ADMIN_ROLE)]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _profesoresService.SoftDeleteAsync(id);
            if (!deleted)
                return NotFound();
            return NoContent();
        }
    }
}
