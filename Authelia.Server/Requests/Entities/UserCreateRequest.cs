namespace Authelia.Server.Requests.Entities
{
    public class UserCreateRequest
    {
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string UserMail { get; set; }
        public string UserPhone { get; set; }
    }
}
