namespace Authelia.Database.Model
{
    public partial class UserMetumSafeDto
    {
        public string UserId { get; set; }
        public string UserMetaKey { get; set; }
        public string UserMetaValue { get; set; }
        public Authelia.Database.Model.UserSafeDto User { get; set; }
    }
}