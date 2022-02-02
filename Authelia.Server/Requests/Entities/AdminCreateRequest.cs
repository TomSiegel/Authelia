namespace Authelia.Server.Requests.Entities
{
    public class AdminCreateRequest
    {
        public string UserName { get; set; }
        public string UserPassword { get; set; }
    }
}
