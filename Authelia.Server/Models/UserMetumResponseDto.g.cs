namespace Authelia.Database.Model
{
    public partial class UserMetumResponseDto
    {
        public string UserId { get; set; }
        public string UserMetaKey { get; set; }
        public string UserMetaValue { get; set; }
    }
}