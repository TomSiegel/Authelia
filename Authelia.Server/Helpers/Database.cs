namespace Authelia.Server.Helpers
{
    public static class DbHelpers
    {
        public static string CreateGuid()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}
