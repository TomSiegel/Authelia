namespace Authelia.Server.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AllowAnonymousAttribute : Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute
    {
    }
}
