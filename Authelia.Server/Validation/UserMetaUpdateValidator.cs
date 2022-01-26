using FluentValidation;
using Authelia.Database.Model;

namespace Authelia.Server.Validation
{
    public class UserMetaUpdateValidator : AbstractValidator<UserMetumDto>
    {
        public UserMetaUpdateValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.UserMetaKey).NotEmpty();
            RuleFor(x => x.UserMetaValue).NotEmpty();
        }
    }

    public class UserMetaSafeDtoUpdateValidator : AbstractValidator<UserMetumSafeDto>
    {
        public UserMetaSafeDtoUpdateValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.UserMetaKey).NotEmpty();
            RuleFor(x => x.UserMetaValue).NotEmpty();
        }
    }
}
