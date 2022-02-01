using FluentValidation;
using Authelia.Server.Requests.Entities;

namespace Authelia.Server.Validation
{
    public class UserMetaUpdateValidator : AbstractValidator<UserMetaUpdateRequest>
    {
        public UserMetaUpdateValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.UserMetaKey).NotEmpty();
            RuleFor(x => x.UserMetaValue).NotEmpty();
        }
    }
}
