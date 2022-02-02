using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authentication;
using Authelia.Server.Authentication;
using Authelia.Server.Exceptions;

namespace Authelia.Server.Authorization
{
    public abstract class AsyncAuthorizationFilter : IAsyncAuthorizationFilter
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
                        await AuthorizeAsync(context);

                        return;
                    }
                        
                }

                context.Result = new UnauthorizedObjectResult(new ErrorResponse()
                {
                    Code = ErrorCodes.C_UnauthorizedUser,
                    Message = "please provide a valid authentication key or login first"
                });
            }

            await AuthorizeAsync(context);
        }

        public abstract Task AuthorizeAsync(AuthorizationFilterContext context);
    }
}
