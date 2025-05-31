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
    public class AsignacionProyectoController : ControllerBase
    {
        private readonly IAsignacionProyectoService _service;
        public AsignacionProyectoController(IAsignacionProyectoService service)
        {
            _service = service;
        }

        [HttpGet]
        [RoleAuthorize(RoleHelper.ADMIN_ROLE, RoleHelper.PROFESOR_ROLE)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        [RoleAuthorize(RoleHelper.ADMIN_ROLE, RoleHelper.PROFESOR_ROLE)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpPost]
        [RoleAuthorize(RoleHelper.ADMIN_ROLE)]
        public async Task<IActionResult> Create([FromBody] AsignacionProyectoCreateDTO dto)
        {
            var id = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id }, dto);
        }

        [HttpPut("{id}")]
        [RoleAuthorize(RoleHelper.ADMIN_ROLE)]
        public async Task<IActionResult> Update(int id, [FromBody] AsignacionProyectoDTO dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            if (!updated)
                return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [RoleAuthorize(RoleHelper.ADMIN_ROLE)]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.SoftDeleteAsync(id);
            if (!deleted)
                return NotFound();
            return NoContent();
        }
    }
}
