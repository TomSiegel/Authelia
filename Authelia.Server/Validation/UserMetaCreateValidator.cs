using FluentValidation;
using Authelia.Database.Model;

namespace Authelia.Server.Validation
{
    public class UserMetaCreateValidator : AbstractValidator<UserMetumDto>
    {
        public UserMetaCreateValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.UserMetaKey).NotEmpty();
            RuleFor(x => x.UserMetaValue).NotEmpty();
        }
    }

    public class UserMetaSafeDtoCreateValidator : AbstractValidator<UserMetumSafeDto>
    {
        public UserMetaSafeDtoCreateValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.UserMetaKey).NotEmpty();
            RuleFor(x => x.UserMetaValue).NotEmpty();
        }
    }
}
