using FluentValidation;
using Authelia.Server.Requests.Entities;
using Authelia.Server.Extensions;
using Authelia.Server.Security;

namespace Authelia.Server.Validation
{
    public class UserCreateValidator : AbstractValidator<UserCreateRequest>
    {
        public UserCreateValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().Matches("^[a-zA-Z0-9_-]{2,30}$");
            RuleFor(x => x.UserPassword).NotEmpty().Password(PasswordSecuritySettings.Default);
            RuleFor(x => x.UserMail).EmailAddress(FluentValidation.Validators.EmailValidationMode.AspNetCoreCompatible);
            RuleFor(x => x.UserPhone).PhoneNumber();
            RuleFor(x => x).Custom((user, context) =>
            {
                if (string.IsNullOrEmpty(user.UserMail) && string.IsNullOrEmpty(user.UserPhone))
                {
                    context.AddFailure("You have to specify either an e-mail address or a phone number");
                }
            });
        }
    }
}
