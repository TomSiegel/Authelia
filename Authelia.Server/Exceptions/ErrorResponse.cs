using Mapster;

namespace Authelia.Server.Exceptions
{
    public class ErrorResponse
    {
        public string? Message { get; set; }

        public string? Code { get; set; }

        public object? Data { get; set; }

        public ErrorResponse? InnerError { get; set; }
    }
}
