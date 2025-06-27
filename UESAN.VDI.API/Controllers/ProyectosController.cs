using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UESAN.VDI.CORE.Core.DTOs;
using UESAN.VDI.CORE.Core.Interfaces;
using UESAN.VDI.CORE.Core.Helpers;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;

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

        [HttpPost("crear-masivo-excel")]
        [RoleAuthorize(RoleHelper.ADMIN_ROLE)]
        public async Task<IActionResult> CrearMasivoExcel(IFormFile file, 
            [FromServices] ILineasInvestigacionService lineasService)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No se ha proporcionado un archivo válido.");

            try
            {
                // Obtener todas las líneas de investigación para el mapeo
                var lineasInvestigacion = await lineasService.GetAllAsync();
                var lineasMap = lineasInvestigacion.ToDictionary(
                    l => l.Nombre.Trim().ToLower(), 
                    l => l.LineaId
                );

                var adminCrea = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
                using var package = new ExcelPackage(file.OpenReadStream());
                var worksheet = package.Workbook.Worksheets[0];
                var proyectos = new List<ProyectoCreateDTO>();
                var errores = new List<string>();

                // Leer desde la fila 2 (asumiendo que la fila 1 tiene encabezados)
                for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                {
                    if (string.IsNullOrWhiteSpace(worksheet.Cells[row, 1].Text))
                        continue;

                    var lineaNombre = worksheet.Cells[row, 6].Text.Trim().ToLower();
                    int? lineaId = null;

                    if (!string.IsNullOrWhiteSpace(lineaNombre))
                    {
                        if (lineasMap.TryGetValue(lineaNombre, out int id))
                        {
                            lineaId = id;
                        }
                        else
                        {
                            errores.Add($"Fila {row}: La línea de investigación '{worksheet.Cells[row, 6].Text}' no existe en el sistema.");
                            continue;
                        }
                    }

                    var proyecto = new ProyectoCreateDTO
                    {
                        Titulo = worksheet.Cells[row, 1].Text,
                        Descripcion = worksheet.Cells[row, 2].Text,
                        FechaInicio = DateTime.TryParse(worksheet.Cells[row, 3].Text, out var fecha) ? fecha : DateTime.Now,
                        Estatus = worksheet.Cells[row, 4].Text,
                        Recomendado = bool.TryParse(worksheet.Cells[row, 5].Text, out var recomendado) && recomendado,
                        LineaId = lineaId
                    };
                    proyectos.Add(proyecto);
                }

                if (errores.Any())
                {
                    return BadRequest(new { 
                        Mensaje = "Existen errores en algunas líneas de investigación.",
                        Errores = errores
                    });
                }

                var (created, failed) = await _proyectosService.CrearMasivoAsync(proyectos, adminCrea);
                return Ok(new { 
                    ProyectosCreados = created, 
                    ProyectosFallidos = failed,
                    MensajeResumen = $"Se crearon {created} proyectos correctamente. {failed} proyectos no pudieron ser creados."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al procesar el archivo: {ex.Message}");
            }
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
