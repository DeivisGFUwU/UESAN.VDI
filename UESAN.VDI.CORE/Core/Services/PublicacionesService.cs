using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UESAN.VDI.CORE.Core.DTOs;
using UESAN.VDI.CORE.Core.Entities;
using UESAN.VDI.CORE.Core.Interfaces;

namespace UESAN.VDI.CORE.Core.Services
{
    public class PublicacionesService : IPublicacionesService
    {
        private readonly IPublicacionesRepository _publicacionesRepository;
        private readonly IProfesoresRepository _profesoresRepository;
        private readonly IRevistasRepository _revistasRepository;
        public PublicacionesService(IPublicacionesRepository publicacionesRepository, IProfesoresRepository profesoresRepository, IRevistasRepository revistasRepository)
        {
            _publicacionesRepository = publicacionesRepository;
            _profesoresRepository = profesoresRepository;
            _revistasRepository = revistasRepository;
        }

        // Roles: 1 = Administrador, 2 = Profesor, 3 = Usuario Normal
        private const string ADMIN_ROLE = "1";
        private const string PROFESOR_ROLE = "2";
        private const string NORMAL_ROLE = "3";

        public async Task<List<PublicacionListDTO>> GetAllAsync()
        {
            var publicaciones = await _publicacionesRepository.GetAllAsync();
            return publicaciones.Select(p => new PublicacionListDTO
            {
                PublicacionId = p.PublicacionId,
                Titulo = p.Titulo,
                FechaPublicacion = p.FechaPublicacion
            }).ToList();
        }

        public async Task<List<PublicacionListDTO>> GetAllFilteredAsync(string? userRole, string? userId, int? profesorId, string? issn, DateTime? fechaInicio, DateTime? fechaFin)
        {
            var publicaciones = await _publicacionesRepository.GetAllAsync();
            if (profesorId.HasValue)
                publicaciones = publicaciones.Where(p => p.ProfesorId == profesorId.Value).ToList();
            if (!string.IsNullOrEmpty(issn))
                publicaciones = publicaciones.Where(p => p.Issn == issn).ToList();
            if (fechaInicio.HasValue)
                publicaciones = publicaciones.Where(p => p.FechaPublicacion >= fechaInicio.Value).ToList();
            if (fechaFin.HasValue)
                publicaciones = publicaciones.Where(p => p.FechaPublicacion <= fechaFin.Value).ToList();
            if (userRole == NORMAL_ROLE)
            {
                publicaciones = publicaciones.Where(p => p.IssnNavigation != null && p.IssnNavigation.Activa).ToList();
            }
            else if (userRole == PROFESOR_ROLE && int.TryParse(userId, out int usuarioId))
            {
                var profesor = await _profesoresRepository.GetAllActivosAsync();
                var prof = profesor.FirstOrDefault(x => x.UsuarioId == usuarioId);
                if (prof != null)
                    publicaciones = publicaciones.Where(p => p.ProfesorId == prof.ProfesorId).ToList();
                else
                    publicaciones = new List<Publicaciones>();
            }
            return publicaciones.Select(p => new PublicacionListDTO
            {
                PublicacionId = p.PublicacionId,
                Titulo = p.Titulo,
                FechaPublicacion = p.FechaPublicacion
            }).ToList();
        }

        public async Task<PublicacionDTO?> GetByIdAsync(int id)
        {
            var publicacion = await _publicacionesRepository.GetByIdAsync(id);
            if (publicacion == null) return null;
            return new PublicacionDTO
            {
                PublicacionId = publicacion.PublicacionId,
                ProfesorId = publicacion.ProfesorId,
                Issn = publicacion.Issn,
                Titulo = publicacion.Titulo,
                FechaPublicacion = publicacion.FechaPublicacion,
                Puntaje = publicacion.Puntaje
            };
        }

        public async Task<PublicacionDTO?> GetByIdFilteredAsync(int id, string? userRole, string? userId)
        {
            var publicacion = await _publicacionesRepository.GetByIdAsync(id);
            if (publicacion == null)
                return null;
            if (userRole == NORMAL_ROLE && !publicacion.IssnNavigation.Activa)
                return null;
            if (userRole == PROFESOR_ROLE && int.TryParse(userId, out int usuarioId))
            {
                var profesor = await _profesoresRepository.GetAllActivosAsync();
                var prof = profesor.FirstOrDefault(x => x.UsuarioId == usuarioId);
                if (prof == null || publicacion.ProfesorId != prof.ProfesorId)
                    return null;
            }
            return new PublicacionDTO
            {
                PublicacionId = publicacion.PublicacionId,
                ProfesorId = publicacion.ProfesorId,
                Issn = publicacion.Issn,
                Titulo = publicacion.Titulo,
                FechaPublicacion = publicacion.FechaPublicacion,
                Puntaje = publicacion.Puntaje
            };
        }

        public async Task<int> CreateAsync(PublicacionCreateDTO dto)
        {
            var publicacion = new Publicaciones
            {
                ProfesorId = dto.ProfesorId,
                Issn = dto.Issn,
                Titulo = dto.Titulo,
                FechaPublicacion = dto.FechaPublicacion,
                Puntaje = dto.Puntaje
            };
            return await _publicacionesRepository.CreateAsync(publicacion);
        }

        public async Task<int> CreateValidatedAsync(PublicacionCreateDTO dto, string? userRole, string? userId)
        {
            var profesor = await _profesoresRepository.GetByIdAsync(dto.ProfesorId);
            var revista = await _revistasRepository.GetByIssnAsync(dto.Issn);
            if (profesor == null || revista == null)
                return -1;
            if (userRole == PROFESOR_ROLE && int.TryParse(userId, out int usuarioId))
            {
                if (profesor.UsuarioId != usuarioId)
                    return -1;
            }
            var publicacion = new Publicaciones
            {
                ProfesorId = dto.ProfesorId,
                Issn = dto.Issn,
                Titulo = dto.Titulo,
                FechaPublicacion = dto.FechaPublicacion,
                Puntaje = dto.Puntaje
            };
            return await _publicacionesRepository.CreateAsync(publicacion);
        }

        public async Task<bool> UpdateAsync(int id, PublicacionDTO dto)
        {
            var publicacion = await _publicacionesRepository.GetByIdAsync(id);
            if (publicacion == null) return false;
            publicacion.ProfesorId = dto.ProfesorId;
            publicacion.Issn = dto.Issn;
            publicacion.Titulo = dto.Titulo;
            publicacion.FechaPublicacion = dto.FechaPublicacion;
            publicacion.Puntaje = dto.Puntaje;
            return await _publicacionesRepository.UpdateAsync(publicacion);
        }

        public async Task<bool> UpdateValidatedAsync(int id, PublicacionDTO dto, string? userRole, string? userId)
        {
            var publicacion = await _publicacionesRepository.GetByIdAsync(id);
            if (publicacion == null)
                return false;
            var profesor = await _profesoresRepository.GetByIdAsync(dto.ProfesorId);
            var revista = await _revistasRepository.GetByIssnAsync(dto.Issn);
            if (profesor == null || revista == null)
                return false;
            if (userRole == PROFESOR_ROLE && int.TryParse(userId, out int usuarioId))
            {
                if (profesor.UsuarioId != usuarioId)
                    return false;
            }
            publicacion.ProfesorId = dto.ProfesorId;
            publicacion.Issn = dto.Issn;
            publicacion.Titulo = dto.Titulo;
            publicacion.FechaPublicacion = dto.FechaPublicacion;
            publicacion.Puntaje = dto.Puntaje;
            return await _publicacionesRepository.UpdateAsync(publicacion);
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            return await _publicacionesRepository.SoftDeleteAsync(id);
        }

        public async Task<bool> SoftDeleteValidatedAsync(int id, string? userRole, string? userId)
        {
            var publicacion = await _publicacionesRepository.GetByIdAsync(id);
            if (publicacion == null)
                return false;
            var profesor = await _profesoresRepository.GetByIdAsync(publicacion.ProfesorId);
            if (userRole == PROFESOR_ROLE && int.TryParse(userId, out int usuarioId))
            {
                if (profesor == null || profesor.UsuarioId != usuarioId)
                    return false;
            }
            return await _publicacionesRepository.SoftDeleteAsync(id);
        }
    }
}
