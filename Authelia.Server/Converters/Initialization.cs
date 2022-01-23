using Mapster;
using Authelia.Server.Exceptions;
using Authelia.Database.Model;
using FluentValidation.Results;

namespace Authelia.Server.Converters
{
    public class Initialization : ICodeGenerationRegister, IRegister
    {
        public void Register(CodeGenerationConfig config)
        {
            config.AdaptTo("[name]Dto")
                .ForType<UserMetum>()
                .ForType<UserToken>()
                .ForType<User>(options =>
                {
                    options.Ignore(x => x.UserTokens);
                    options.Ignore(x => x.UserMeta);
                });

            config.AdaptTo("[name]SafeDto")
                .ForType<UserMetum>()
                .ForType<UserToken>(options =>
                {
                    options.Ignore(x => x.TokenCreatorIp);
                    options.Ignore(x => x.User);
                })
                .ForType<User>(options =>
                {
                    options.Ignore(x => x.UserPassword);
                    options.Ignore(x => x.UserTokens);
                    options.Ignore(x => x.UserMeta);
                });

            //config.GenerateMapper("[name]Mapper")
            //    .ForType<UserMetum>()
            //    .ForType<UserToken>()
            //    .ForType<User>();
        }

        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Exception, ErrorResponse>()
                .Map(d => d.Message, s => s.Message)
                .Map(d => d.InnerError, s => s.InnerException);

            config.NewConfig<ValidationResult, ErrorResponse>()
                .Map(d => d.Message, s => String.Join(Environment.NewLine, s.Errors.Select(err => err.ToString())));
        }
    }
}
