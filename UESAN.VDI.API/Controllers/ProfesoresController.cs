using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UESAN.VDI.CORE.Core.DTOs;
using UESAN.VDI.CORE.Core.Interfaces;
using UESAN.VDI.CORE.Core.Helpers;
using OfficeOpenXml; // Necesitas instalar EPPlus
using System.Collections.Generic;
using System;

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

        // PUT: api/Profesores/reactivar/{id}
        [HttpPut("reactivar/{id}")]
        [RoleAuthorize(RoleHelper.ADMIN_ROLE)]
        public async Task<IActionResult> Reactivar(int id)
        {
            var result = await _profesoresService.ReactivateAsync(id);
            if (!result)
                return NotFound();
            return Ok();
        }

        // POST: api/Profesores/crear-profesor-usuario
        [HttpPost("crear-profesor-usuario")]
        [RoleAuthorize(RoleHelper.ADMIN_ROLE)]
        public async Task<IActionResult> CrearProfesorConUsuario([FromBody] ProfesorUsuarioCreateDTO dto)
        {
            var (success, errorMessage) = await _profesoresService.CrearProfesorConUsuarioDebugAsync(dto);
            if (!success)
                return BadRequest(errorMessage ?? "No se pudo crear el profesor y usuario. Verifique los datos.");
            return Ok();
        }

        // POST: api/Profesores/crear-profesores-usuarios-masivo
        [HttpPost("crear-profesores-usuarios-masivo")]
        [RoleAuthorize(RoleHelper.ADMIN_ROLE)]
        public async Task<IActionResult> CrearProfesoresConUsuariosMasivo([FromBody] List<ProfesorUsuarioCreateDTO> dtos)
        {
            var resultado = await _profesoresService.CrearProfesoresConUsuariosMasivoAsync(dtos);
            return Ok(resultado);
        }

        // POST: api/Profesores/crear-profesores-usuarios-masivo-excel
        [HttpPost("crear-profesores-usuarios-masivo-excel")]
        [RoleAuthorize(RoleHelper.ADMIN_ROLE)]
        public async Task<IActionResult> CrearProfesoresConUsuariosMasivoExcel([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No se ha proporcionado un archivo válido.");
            try
            {
                using var package = new ExcelPackage(file.OpenReadStream());
                var worksheet = package.Workbook.Worksheets[0];
                var profesores = new List<ProfesorUsuarioCreateDTO>();
                for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                {
                    var profesor = new ProfesorUsuarioCreateDTO
                    {
                        Nombre = worksheet.Cells[row, 1].Text,
                        Apellido = worksheet.Cells[row, 2].Text,
                        Correo = worksheet.Cells[row, 3].Text,
                        RoleId = int.TryParse(worksheet.Cells[row, 4].Text, out int roleId) ? roleId : 0,
                        Departamento = worksheet.Cells[row, 5].Text,
                        Categoria = worksheet.Cells[row, 6].Text
                    };
                    profesores.Add(profesor);
                }
                var resultado = await _profesoresService.CrearProfesoresConUsuariosMasivoAsync(profesores);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al procesar el archivo: {ex.Message}");
            }
        }
    }
}
