namespace Authelia.Database.Model
{
    public partial class UserTokenSafeDto
    {
        public string UserTokenId { get; set; }
        public string UserId { get; set; }
        public System.DateTime? TokenExpiration { get; set; }
        public System.DateTime? TokenCreatedUtc { get; set; }
    }
}