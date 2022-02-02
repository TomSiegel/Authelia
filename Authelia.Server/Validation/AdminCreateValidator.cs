using FluentValidation;
using Authelia.Server.Requests.Entities;
using Authelia.Server.Extensions;
using Authelia.Server.Security;

namespace Authelia.Server.Validation
{
    public class AdminCreateValidator : AbstractValidator<AdminCreateRequest>
    {
        public AdminCreateValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().Username();
            RuleFor(x => x.UserPassword).NotEmpty().Password(PasswordSecuritySettings.Admin);
        }
    }
}
