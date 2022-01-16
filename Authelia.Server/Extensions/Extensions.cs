using Authelia.Server.Exceptions;
using Authelia.Server.Security;
using Authelia.Database.Model;
using FluentValidation;

namespace Authelia.Server.Extensions
{
    public static class Extensions
    {
        
        public static ErrorResponse WithMessage(this ErrorResponse response, string message)
        {
            response.Message = message;
            return response;
       }

        public static ErrorResponse WithData(this ErrorResponse response, object data)
        {
            response.Data = data;
            return response;
        }

        public static ErrorResponse WithCode(this ErrorResponse response, string code)
        {
            response.Code = code;
            return response;
        }

        public static ErrorResponse WithInnerError(this ErrorResponse response, ErrorResponse error)
        {
            response.InnerError = error;
            return response;
        }



        public  static IRuleBuilderOptions<UserDto, string> PhoneNumber(this IRuleBuilderOptions<UserDto, string> builder)
        {
            return builder.Matches(@"((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}");
        }

        public static IRuleBuilder<UserDto, string> PhoneNumber(this IRuleBuilder<UserDto, string> builder)
        {
            return builder.Matches(@"((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}");
        }

        public static IRuleBuilderOptions<UserDto, string> Password(this IRuleBuilderOptions<UserDto, string> builder, PasswordSecuritySettings settings)
        {
            return builder.Matches(settings.BuildRegex());
        }

        public static IRuleBuilder<UserDto, string> Password(this IRuleBuilder<UserDto, string> builder, PasswordSecuritySettings settings)
        {
            return builder.Matches(settings.BuildRegex());
        }

       
    }
}
