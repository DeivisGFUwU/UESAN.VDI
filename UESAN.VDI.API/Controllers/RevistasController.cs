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
    public class RevistasController : ControllerBase
    {
        private readonly IRevistasService _revistasService;
        public RevistasController(IRevistasService revistasService)
        {
            _revistasService = revistasService;
        }

        [HttpGet]
        [RoleAuthorize(RoleHelper.ADMIN_ROLE, RoleHelper.PROFESOR_ROLE, RoleHelper.NORMAL_ROLE)]
        public async Task<IActionResult> GetAllActivas()
        {
            var result = await _revistasService.GetAllActivasAsync();
            return Ok(result);
        }

        [HttpGet("{issn}")]
        [RoleAuthorize(RoleHelper.ADMIN_ROLE, RoleHelper.PROFESOR_ROLE, RoleHelper.NORMAL_ROLE)]
        public async Task<IActionResult> GetByIssn(string issn)
        {
            var result = await _revistasService.GetByIssnAsync(issn);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpPost]
        [RoleAuthorize(RoleHelper.ADMIN_ROLE)]
        public async Task<IActionResult> Create([FromBody] RevistaDTO dto)
        {
            var id = await _revistasService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetByIssn), new { issn = dto.Issn }, dto);
        }

        [HttpPut("{issn}")]
        [RoleAuthorize(RoleHelper.ADMIN_ROLE)]
        public async Task<IActionResult> Update(string issn, [FromBody] RevistaDTO dto)
        {
            var updated = await _revistasService.UpdateAsync(issn, dto);
            if (!updated)
                return NotFound();
            return NoContent();
        }

        [HttpDelete("{issn}")]
        [RoleAuthorize(RoleHelper.ADMIN_ROLE)]
        public async Task<IActionResult> Delete(string issn)
        {
            var deleted = await _revistasService.SoftDeleteAsync(issn);
            if (!deleted)
                return NotFound();
            return NoContent();
        }
    }
}
