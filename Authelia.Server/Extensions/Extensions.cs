using Authelia.Server.Exceptions;
using Authelia.Server.Security;
using Authelia.Server.Authentication;
using Authelia.Database.Model;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Diagnostics;

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

        public static UserTokenResponseDto WithToken(this UserTokenResponseDto userToken, string token)
        {
            userToken.UserTokenId = token;
            return userToken;
        }

        public static IEnumerable<TOut> AdaptList<TIn, TOut>(this IEnumerable<TIn> source)
        {
            foreach (var item in source)
            {
                yield return item.Adapt<TOut>();
            }
        }

        public static IRuleBuilderOptions<T, string> PhoneNumber<T>(this IRuleBuilderOptions<T, string> builder)
        {
            return builder.Matches(@"((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}");
        }

        public static IRuleBuilder<T, string> PhoneNumber<T>(this IRuleBuilder<T, string> builder)
        {
            return builder.Matches(@"((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}");
        }

        public static IRuleBuilderOptions<T, string> Username<T>(this IRuleBuilderOptions<T, string> builder)
        {
            return builder.Matches("^[a-zA-Z0-9_-]{2,30}$");
        }

        public static IRuleBuilder<T, string> Username<T>(this IRuleBuilder<T, string> builder)
        {
            return builder.Matches("^[a-zA-Z0-9_-]{2,30}$");
        }

        public static IRuleBuilderOptions<T, string> Password<T>(this IRuleBuilderOptions<T, string> builder, PasswordSecuritySettings settings)
        {
            return builder.Matches(settings.BuildRegex());
        }

        public static IRuleBuilder<T, string> Password<T>(this IRuleBuilder<T, string> builder, PasswordSecuritySettings settings)
        {
            return builder.Matches(settings.BuildRegex());
        }

        public static IServiceCollection AddAutheliaIdentity(this IServiceCollection services)
        {
            services.AddIdentity<AutheliaUser, AutheliaRole>().AddDefaultTokenProviders();
            services.AddTransient<IUserStore<AutheliaUser>, AutheliaUserStore>();
            services.AddTransient<IRoleStore<AutheliaRole>, AutheliaRoleStore>();
            return services;
        }

        public static IServiceCollection AddAutheliaAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = AutheliaSchemes.DefaultScheme;
                options.DefaultChallengeScheme = AutheliaSchemes.DefaultScheme;
                options.DefaultForbidScheme = AutheliaSchemes.DefaultScheme;
                options.DefaultSignInScheme = AutheliaSchemes.DefaultScheme;
                options.DefaultSignOutScheme = AutheliaSchemes.DefaultScheme;
                options.DefaultAuthenticateScheme = AutheliaSchemes.DefaultScheme;
            })
                .AddCookie(AutheliaSchemes.CookieScheme, options =>
                {
                    options.LoginPath = new PathString("/authentication/login");
                    options.LogoutPath = new PathString("/authentication/logout");
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                    options.Cookie.Name = "AutheliaSession";
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SameSite = SameSiteMode.None;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                })
                .AddScheme<AutheliaAuthenticationOptions, AutheliaAuthenticationHandler>(AutheliaSchemes.AutheliaScheme, options => { });

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

        public static IApplicationBuilder UseAutheliaExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(c => c.Run(async context =>
            {
                var exception = context.Features
                    .Get<IExceptionHandlerPathFeature>()
                    .Error;
                var response = exception
                    .Adapt<ErrorResponse>()
                    .WithCode(ErrorCodes.S_UnknownServerError);
                await context.Response.WriteAsJsonAsync(response);
            }));
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

        public static string GetUserId(this ClaimsPrincipal principal)
        {
            return principal.GetClaim(ClaimConstants.UserIdentifier);
        }

        public static string GetUserId(this HttpContext context)
        {
            return context?.User?.GetClaim(ClaimConstants.UserIdentifier);
        }
    }
}
