using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UESAN.VDI.CORE.Core.DTOs;
using UESAN.VDI.CORE.Core.Entities;
using UESAN.VDI.CORE.Core.Interfaces;

namespace UESAN.VDI.CORE.Core.Services
{
    public class RevistasService : IRevistasService
    {
        private readonly IRevistasRepository _revistasRepository;
        public RevistasService(IRevistasRepository revistasRepository)
        {
            _revistasRepository = revistasRepository;
        }

        // Roles: 1 = Administrador, 2 = Profesor, 3 = Usuario Normal
        private const string ADMIN_ROLE = "1";
        private const string PROFESOR_ROLE = "2";
        private const string NORMAL_ROLE = "3";

        public async Task<List<RevistaListDTO>> GetAllActivasAsync()
        {
            var revistas = await _revistasRepository.GetAllActivasAsync();
            return revistas.Select(r => new RevistaListDTO
            {
                Issn = r.Issn,
                Titulo = r.Titulo
            }).ToList();
        }

        public async Task<List<RevistaListDTO>> GetAllByRoleAsync(string? userRole)
        {
            List<Revistas> revistas;
            if (userRole == ADMIN_ROLE)
                revistas = await _revistasRepository.GetAllAsync();
            else
                revistas = await _revistasRepository.GetAllActivasAsync();
            return revistas.Select(r => new RevistaListDTO
            {
                Issn = r.Issn,
                Titulo = r.Titulo
            }).ToList();
        }

        public async Task<RevistaDTO?> GetByIssnAsync(string issn, string? userRole)
        {
            Revistas? revista;
            if (userRole == ADMIN_ROLE)
                revista = await _revistasRepository.GetByIssnAsync(issn, includeInactive: true);
            else
                revista = await _revistasRepository.GetByIssnAsync(issn);
            if (revista == null) return null;
            return new RevistaDTO
            {
                Issn = revista.Issn,
                Titulo = revista.Titulo,
                Categoria = revista.Categoria,
                Cuartil = revista.Cuartil,
                Activa = revista.Activa,
                Pais = revista.Pais
            };
        }

        public async Task<string> CreateAsync(RevistaDTO dto)
        {
            var revista = new Revistas
            {
                Issn = dto.Issn,
                Titulo = dto.Titulo,
                Categoria = dto.Categoria,
                Cuartil = dto.Cuartil,
                Activa = true,
                Pais = dto.Pais
            };
            return await _revistasRepository.CreateAsync(revista);
        }

        public async Task<bool> UpdateAsync(string issn, RevistaDTO dto)
        {
            var revista = await _revistasRepository.GetByIssnAsync(issn, includeInactive: true);
            if (revista == null) return false;
            revista.Titulo = dto.Titulo;
            revista.Categoria = dto.Categoria;
            revista.Cuartil = dto.Cuartil;
            revista.Pais = dto.Pais;
            return await _revistasRepository.UpdateAsync(revista);
        }

        public async Task<bool> SoftDeleteAsync(string issn)
        {
            return await _revistasRepository.SoftDeleteAsync(issn);
        }
    }
}
