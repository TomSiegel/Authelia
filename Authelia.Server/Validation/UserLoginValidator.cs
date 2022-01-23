using FluentValidation;
using Authelia.Server.Requests.Entities;
using Authelia.Server.Extensions;
using Authelia.Server.Security;

namespace Authelia.Server.Validation
{
    public class UserLoginValidator : AbstractValidator<LoginUser>
    {
        public UserLoginValidator()
        {
            RuleFor(x => x.UserName).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
