namespace Authelia.Server.Requests.Entities
{
    public class UserMetaCreateRequest
    {
        public string UserId { get; set; }
        public string UserMetaKey { get; set; }
        public string UserMetaValue { get; set; }
    }
}
