using Authelia.Server.Exceptions;

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
    }
}
