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
            config.AdaptTwoWays("[name]Dto")
                .ForType<User>()
                .ForType<UserMetum>();

            config.GenerateMapper("[name]Mapper")
                .ForType<User>()
                .ForType<UserMetum>();
        }

        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Exception, ErrorResponse>()
                .Map(d => d.Message, s => s.Message + "iasndia")
                .Map(d => d.InnerError, s => s.InnerException);

            config.NewConfig<ValidationResult, ErrorResponse>()
                .Map(d => d.Message, s => String.Join(Environment.NewLine, s.Errors.Select(err => err.ToString())));
        }
    }
}
