using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authentication;
using Authelia.Server.Authentication;
using Authelia.Server.Exceptions;

namespace Authelia.Server.Authorization
{
    public abstract class AuthorizationFilter : IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (!(context.HttpContext.User.Identity?.IsAuthenticated ?? false))
            {
                foreach (var item in AutheliaSchemes.SupportedSchemesList)
                {
                    var result = await context.HttpContext.AuthenticateAsync(item);

                    if (result.Succeeded)
                    {
                        Authorize(context);

                        return;
                    }
                        
                }

                context.Result = new UnauthorizedObjectResult(new ErrorResponse()
                {
                    Code = ErrorCodes.C_UnauthorizedUser,
                    Message = "please provide a valid authentication key or login first"
                });
            }

            Authorize(context);
        }

        public abstract void Authorize(AuthorizationFilterContext context);
    }
}
