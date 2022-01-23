namespace Authelia.Database.Model
{
    public partial class UserTokenDto
    {
        public string UserTokenId { get; set; }
        public string UserId { get; set; }
        public System.DateTime? TokenExpiration { get; set; }
        public System.DateTime? TokenCreatedUtc { get; set; }
        public string TokenCreatorIp { get; set; }
        public Authelia.Database.Model.UserDto User { get; set; }
    }
}