using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authentication;
using Authelia.Server.Authentication;
using Authelia.Server.Exceptions;

namespace Authelia.Server.Authorization
{
    public class AuthorizeAttribute : TypeFilterAttribute
    {
        public AuthorizeAttribute() : base(typeof(AuthorizeFilter))
        {
        }

    }

    public class AuthorizeFilter : AuthorizationFilter
    {
        public override void Authorize(AuthorizationFilterContext context)
        {
        }
    }
}
