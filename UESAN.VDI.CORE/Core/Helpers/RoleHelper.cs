namespace UESAN.VDI.CORE.Core.Helpers
{
    public static class RoleHelper
    {
        public const string ADMIN_ROLE = "3";
        public const string PROFESOR_ROLE = "2";
        public const string NORMAL_ROLE = "1";

        public static bool IsAdmin(string? role) => role == ADMIN_ROLE;
        public static bool IsProfesor(string? role) => role == PROFESOR_ROLE;
        public static bool IsNormal(string? role) => role == NORMAL_ROLE;
    }
}