using Authelia.Server.Exceptions;
using Authelia.Server.Requests.Entities;
using Authelia.Database.Model;
using FluentValidation.Results;

namespace Authelia.Server.Converters
{
    public class Initialization : Mapster.ICodeGenerationRegister, Mapster.IRegister
    {
        public void Register(Mapster.CodeGenerationConfig config)
        {
            config.AdaptTo("[name]ResponseDto")
                .ForType<UserMetum>(options =>
                {
                    options.Ignore(x => x.User);
                })
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

        public void Register(Mapster.TypeAdapterConfig config)
        {
            config.NewConfig<Exception, ErrorResponse>()
                .Map(d => d.Message, s => s.Message)
                .Map(d => d.InnerError, s => s.InnerException);

            config.NewConfig<ValidationResult, ErrorResponse>()
                .Map(d => d.Message, s => string.Join(Environment.NewLine, s.Errors.Select(err => err.ToString())));

            config.NewConfig<UserCreateRequest, User>();
            config.NewConfig<UserMetaCreateRequest, UserMetum>();
            config.NewConfig<UserMetaUpdateRequest, UserMetum>();
        }
    }
}
