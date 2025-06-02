using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UESAN.VDI.CORE.Core.DTOs;
using UESAN.VDI.CORE.Core.Entities;
using UESAN.VDI.CORE.Core.Interfaces;

namespace UESAN.VDI.CORE.Core.Services
{
    public class ProfesoresService : IProfesoresService
    {
        private readonly IProfesoresRepository _profesoresRepository;
        private readonly IUsuariosRepository _usuariosRepository;
        public ProfesoresService(IProfesoresRepository profesoresRepository, IUsuariosRepository usuariosRepository)
        {
            _profesoresRepository = profesoresRepository;
            _usuariosRepository = usuariosRepository;
        }

        public async Task<List<ProfesorListDTO>> GetAllActivosAsync()
        {
            var profesores = await _profesoresRepository.GetAllActivosAsync();
            return profesores.Select(p => new ProfesorListDTO
            {
                ProfesorId = p.ProfesorId,
                Departamento = p.Departamento,
                Categoria = p.Categoria
            }).ToList();
        }

        public async Task<ProfesorDTO?> GetByIdAsync(int id)
        {
            var profesor = await _profesoresRepository.GetByIdAsync(id);
            if (profesor == null) return null;
            return new ProfesorDTO
            {
                ProfesorId = profesor.ProfesorId,
                UsuarioId = profesor.UsuarioId,
                Departamento = profesor.Departamento,
                FechaIngreso = profesor.FechaIngreso,
                Categoria = profesor.Categoria
            };
        }

        public async Task<int> CreateAsync(ProfesorCreateDTO dto)
        {
            // Validar que el usuario exista
            var usuario = await _usuariosRepository.GetByIdAsync(dto.UsuarioId);
            if (usuario == null)
                throw new System.Exception($"No existe un usuario con el id {dto.UsuarioId}");
            var profesor = new Profesores
            {
                UsuarioId = dto.UsuarioId,
                Departamento = dto.Departamento,
                FechaIngreso = dto.FechaIngreso,
                Categoria = dto.Categoria,
                Activo = true
            };
            return await _profesoresRepository.CreateAsync(profesor);
        }

        public async Task<bool> UpdateAsync(int id, ProfesorDTO dto)
        {
            var profesor = await _profesoresRepository.GetByIdAsync(id);
            if (profesor == null) return false;
            profesor.Departamento = dto.Departamento;
            profesor.FechaIngreso = dto.FechaIngreso;
            profesor.Categoria = dto.Categoria;
            return await _profesoresRepository.UpdateAsync(profesor);
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            var profesor = await _profesoresRepository.GetByIdAsync(id);
            if (profesor == null) return false;
            profesor.Activo = false;
            return await _profesoresRepository.UpdateAsync(profesor);
        }
    }
}
