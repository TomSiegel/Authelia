namespace Authelia.Database.Model
{
    public partial class UserDto
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public int UserOrder { get; set; }
        public System.DateTime? UserCreatedUtc { get; set; }
        public string UserCreatorIp { get; set; }
        public byte UserVerified { get; set; }
        public string UserMail { get; set; }
        public string UserPhone { get; set; }
        public System.DateTime? UserDeletedUtc { get; set; }
        public System.Collections.Generic.ICollection<Authelia.Database.Model.UserMetumDto> UserMeta { get; set; }
        public System.Collections.Generic.ICollection<Authelia.Database.Model.UserToken> UserTokens { get; set; }
    }
}