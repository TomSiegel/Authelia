using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Authelia.Server.Authentication
{
    public class AutheliaSchemes
    {
        public const string CookieScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        public const string AutheliaScheme = "Authelia";
        public const string JwtScheme = JwtBearerDefaults.AuthenticationScheme;
        public const string DefaultScheme = CookieScheme;

        public const string SupportedSchemes = AutheliaScheme + "," + CookieScheme;
        public static string[] SupportedSchemesList = new string[] { AutheliaScheme, CookieScheme };
    }
}
