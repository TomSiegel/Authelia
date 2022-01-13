using FluentValidation;
using Authelia.Database.Model;

namespace Authelia.Server.Validation
{
    public class CreateUserValidator : AbstractValidator<UserDto>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().Matches("^[a-zA-Z0-9_-]{2,30}$"); 
            RuleFor(x => x.UserPassword).NotEmpty().Matches("^((?=.*?[A-Z])|(?=.*?[a-z]))(?=.*?[0-9]).{8,50}$");
            RuleFor(x => x.UserMail).NotEmpty().EmailAddress(FluentValidation.Validators.EmailValidationMode.AspNetCoreCompatible);
        }
    }
}
