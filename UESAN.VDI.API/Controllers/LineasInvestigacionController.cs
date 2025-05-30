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
    public class LineasInvestigacionController : ControllerBase
    {
        private readonly ILineasInvestigacionService _service;
        public LineasInvestigacionController(ILineasInvestigacionService service)
        {
            _service = service;
        }

        // Get all (/LineasInvestigacion)
        [HttpGet]
        [RoleAuthorize(RoleHelper.ADMIN_ROLE, RoleHelper.PROFESOR_ROLE)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetlineasInvestigacion();
            return Ok(result);
        }

        // Get by ID ( /LineasInvestigacion/4)
        [HttpGet("{id}")]
        [RoleAuthorize(RoleHelper.ADMIN_ROLE, RoleHelper.PROFESOR_ROLE)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetLineasInvestigacionById(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        // Post (/LineasInvestigacion)
        [HttpPost]
        [RoleAuthorize(RoleHelper.ADMIN_ROLE)]
        public async Task<IActionResult> Create([FromBody] LineasInvestigacionCreateDTO dto)
        {
            var id = await _service.CreateLineasInvestigacion(dto);
            return CreatedAtAction(nameof(GetById), new { id }, dto);
        }

        // Put (/LineasInvestigacion)
        [HttpPut]
        [RoleAuthorize(RoleHelper.ADMIN_ROLE)]
        public async Task<IActionResult> Update([FromBody] LineasInvestigacionListDTO dto)
        {
            var updated = await _service.UpdateLineasInvestigacion(dto);
            if (!updated)
                return NotFound();
            return NoContent();
        }

        // Delete (/LineasInvestigacion/6)
        [HttpDelete("{id}")]
        [RoleAuthorize(RoleHelper.ADMIN_ROLE)]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteLineasInvestigacion(id);
            return NoContent();
        }
    }
}
