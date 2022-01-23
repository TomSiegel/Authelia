namespace Authelia.Server.Helpers
{
    public static class HttpHelper
    {
        public static string GetUserIp(this HttpContext context)
        {
            return context.Connection.RemoteIpAddress?.ToString();
        }
    }
}
