using Authelia.Database.Model;
using Authelia.Server.Authentication;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace Authelia.Server.Authentication
{
    public static class AuthenticationExtensions
    {
        public static ClaimsPrincipal CreatePrincipal(this User user, string scheme)
        {
            var claims = new Claim[] {
                new Claim(ClaimConstants.Username, user.UserName),
                new Claim(ClaimConstants.UserIdentifier, user.UserId),
                new Claim(ClaimConstants.Email, user.UserMail ?? ""),
                new Claim(ClaimConstants.Phone, user.UserPhone ?? ""),
                new Claim(ClaimConstants.Verified, user.UserVerified.ToString())
            };
            var identity = new ClaimsIdentity(claims, scheme);
            var principal = new ClaimsPrincipal(identity);

            if (user.UserIsAdmin == 1)
            {
                identity.AddClaim(new Claim(ClaimConstants.Role, "admin"));
            }

            return principal;
        }

        public static async Task SignInAsync(this User user, HttpContext context, string scheme)
        {
            await context.SignInAsync(user.CreatePrincipal(scheme));
        }

        public static async Task SignInAsync(this User user, HttpContext context)
        {
            await user.SignInAsync(context, AutheliaSchemes.DefaultScheme);
        }

        public static async Task SignInAsync(this HttpContext context, User user, string scheme)
        {
            await user.SignInAsync(context, scheme);
        }

        public static async Task SignInAsync(this HttpContext context, User user)
        {
            await user.SignInAsync(context);
        }
    }
}
