using Authelia.Server.Exceptions;
using Authelia.Server.Security;
using Authelia.Server.Authentication;
using Authelia.Database.Model;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace Authelia.Server.Extensions
{
    public static class Extensions
    {
        
        public static ErrorResponse WithMessage(this ErrorResponse response, string message)
        {
            response.Message = message;
            return response;
       }

        public static ErrorResponse WithData(this ErrorResponse response, object data)
        {
            response.Data = data;
            return response;
        }

        public static ErrorResponse WithCode(this ErrorResponse response, string code)
        {
            response.Code = code;
            return response;
        }

        public static ErrorResponse WithInnerError(this ErrorResponse response, ErrorResponse error)
        {
            response.InnerError = error;
            return response;
        }

        public static UserTokenSafeDto WithToken(this UserTokenSafeDto userToken, string token)
        {
            userToken.UserTokenId = token;
            return userToken;
        }

        public static UserTokenDto WithToken(this UserTokenDto userToken, string token)
        {
            userToken.UserTokenId = token;
            return userToken;
        }


        public  static IRuleBuilderOptions<UserDto, string> PhoneNumber(this IRuleBuilderOptions<UserDto, string> builder)
        {
            return builder.Matches(@"((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}");
        }

        public static IRuleBuilder<UserDto, string> PhoneNumber(this IRuleBuilder<UserDto, string> builder)
        {
            return builder.Matches(@"((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}");
        }

        public static IRuleBuilderOptions<UserDto, string> Password(this IRuleBuilderOptions<UserDto, string> builder, PasswordSecuritySettings settings)
        {
            return builder.Matches(settings.BuildRegex());
        }

        public static IRuleBuilder<UserDto, string> Password(this IRuleBuilder<UserDto, string> builder, PasswordSecuritySettings settings)
        {
            return builder.Matches(settings.BuildRegex());
        }

       
        public static IServiceCollection AddAutheliaAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddScheme<AutheliaAuthenticationOptions, AutheliaAuthenticationHandler>(AutheliaSchemes.AutheliaScheme, options => {

                });
            return services;
        }

        public static IServiceCollection AddAutheliaAuthorization(this IServiceCollection services)
        {      
            services.AddAuthorization();
            return services;
        }

        public static IApplicationBuilder UseAutheliaAuthentication(this IApplicationBuilder app)
        {
            app.UseAuthentication();

            return app;
        }

        public static string GetClaim(this ClaimsPrincipal principal, string type)
        {
            if (principal == null) throw new ArgumentNullException(nameof(principal));

            foreach (var item in principal.Claims)
            {
                if (item.Type == type) return item.Value;
            }

            return null;
        }
    }
}
