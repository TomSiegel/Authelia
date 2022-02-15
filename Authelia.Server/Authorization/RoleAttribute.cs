using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authentication;
using Authelia.Server.Exceptions;

namespace Authelia.Server.Authorization
{
    public class RoleAttribute : TypeFilterAttribute
    {
        public RoleAttribute(string role, string message = $"the user doesn't claim the specified role") : base(typeof(RoleFilter))
        {
            Arguments = new[] { role, message };
        }

    }

    public class RoleFilter : AuthorizationFilter
    {
        private readonly string role;
        private readonly string message;

        public RoleFilter(string role, string message)
        {
            this.role = role;
            this.message = message;
        }

        public override void Authorize(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.IsInRole(role))
            {
                context.Result = new UnauthorizedObjectResult(new ErrorResponse()
                {
                    Code = ErrorCodes.C_UnauthorizedUser,
                    Message = message,
                    Data = "Role = " + role
                });
            }
        }
    }
}
