using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UESAN.VDI.CORE.Core.DTOs;
using UESAN.VDI.CORE.Core.Entities;
using UESAN.VDI.CORE.Core.Interfaces;

namespace UESAN.VDI.CORE.Core.Services
{
    public class FormulariosInvestigacionService : IFormulariosInvestigacionService
    {
        private readonly IFormulariosInvestigacionRepository _repository;
        public FormulariosInvestigacionService(IFormulariosInvestigacionRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<FormularioInvestigacionListDTO>> GetAllAsync()
        {
            var forms = await _repository.GetAllAsync();
            return forms.Select(f => new FormularioInvestigacionListDTO
            {
                FormularioId = f.FormularioId,
                TipoFormulario = f.TipoFormulario,
                Resumen = f.Resumen
            }).ToList();
        }

        public async Task<FormularioInvestigacionDTO?> GetByIdAsync(int id)
        {
            var form = await _repository.GetByIdAsync(id);
            if (form == null) return null;
            return new FormularioInvestigacionDTO
            {
                FormularioId = form.FormularioId,
                ProyectoId = form.ProyectoId,
                TipoFormulario = form.TipoFormulario,
                Resumen = form.Resumen,
                Objetivos = form.Objetivos,
                MedioPublicacion = form.MedioPublicacion,
                Issn = form.Issn,
                Doi = form.Doi,
                Presupuesto = form.Presupuesto,
                Cronograma = form.Cronograma
            };
        }

        public async Task<int> CreateAsync(FormularioInvestigacionCreateDTO dto)
        {
            var entity = new FormulariosInvestigacion
            {
                ProyectoId = dto.ProyectoId,
                TipoFormulario = dto.TipoFormulario,
                Resumen = dto.Resumen,
                Objetivos = dto.Objetivos,
                MedioPublicacion = dto.MedioPublicacion,
                Issn = dto.Issn,
                Doi = dto.Doi,
                Presupuesto = dto.Presupuesto,
                Cronograma = dto.Cronograma,
                Activo = true
            };
            return await _repository.CreateAsync(entity);
        }

        public async Task<bool> UpdateAsync(int id, FormularioInvestigacionDTO dto)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return false;
            entity.TipoFormulario = dto.TipoFormulario;
            entity.Resumen = dto.Resumen;
            entity.Objetivos = dto.Objetivos;
            entity.MedioPublicacion = dto.MedioPublicacion;
            entity.Issn = dto.Issn;
            entity.Doi = dto.Doi;
            entity.Presupuesto = dto.Presupuesto;
            entity.Cronograma = dto.Cronograma;
            return await _repository.UpdateAsync(entity);
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            return await _repository.SoftDeleteAsync(id);
        }
    }
}
