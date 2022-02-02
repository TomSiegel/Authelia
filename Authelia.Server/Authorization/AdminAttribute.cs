using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authentication;
using Authelia.Server.Exceptions;

namespace Authelia.Server.Authorization
{

    public class AdminAttribute : TypeFilterAttribute
    {
        public AdminAttribute() : base(typeof(AdminFilter))
        {
        }

    }

    public class AdminFilter : AuthorizationFilter
    {
        public override void Authorize(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.IsInRole("admin"))
            {
                context.Result = new UnauthorizedObjectResult(new ErrorResponse()
                {
                    Code = ErrorCodes.C_UnauthorizedUser,
                    Message = "you need administrative rights for this action"
                });
            }
        }
    }
}
