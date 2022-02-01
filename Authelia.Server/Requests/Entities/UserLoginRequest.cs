namespace Authelia.Server.Requests.Entities
{
    public class UserLoginRequest
    {
        public string UserName { get; set;}
        public string Password { get; set;}
    }
}
