using UESAN.VDI.CORE.Core.Entities;
using UESAN.VDI.CORE.Core.Settings;

namespace UESAN.VDI.CORE.Core.Interfaces
{
    public interface IJWTService
    {
        JWTSettings _settings { get; }

        string GenerateJWToken(Usuarios user);
    }
}