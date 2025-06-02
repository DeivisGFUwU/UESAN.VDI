using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UESAN.VDI.CORE.Core.Entities;
using UESAN.VDI.CORE.Core.Interfaces;
using UESAN.VDI.CORE.Core.Settings;
using UESAN.VDI.CORE.Core.Helpers;

namespace UESAN.VDI.CORE.Infrastructure.Shared
{
    public class JWTService : IJWTService
    {
        public JWTSettings _settings { get; }
        public JWTService(IOptions<JWTSettings> settings)
        {
            _settings = settings.Value;
        }

        public string GenerateJWToken(Usuarios user)
        {
            var ssk = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey));
            var sc = new SigningCredentials(ssk, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(sc);

            var claims = new[] {
                 new Claim(ClaimTypes.Name, (user.Nombre + " " + user.Apellido)),
                 new Claim(ClaimTypes.GivenName, user.Nombre),
                 new Claim(ClaimTypes.Email, user.Correo),
                 new Claim(ClaimTypes.Role, user.RoleId.ToString()),
                 new Claim(ClaimTypes.NameIdentifier, user.UsuarioId.ToString()), // <-- Asegura que este claim esté presente
                 new Claim("UserId",user.UsuarioId.ToString()),
             };

            var payload = new JwtPayload(
                            _settings.Issuer
                            , _settings.Audience
                            , claims
                            , DateTime.UtcNow
                            , DateTime.UtcNow.AddMinutes(_settings.DurationInMinutes));

            var token = new JwtSecurityToken(header, payload);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
