namespace Authelia.Database.Model
{
    public partial class UserResponseDto
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int UserOrder { get; set; }
        public System.DateTime? UserCreatedUtc { get; set; }
        public string UserCreatorIp { get; set; }
        public byte UserVerified { get; set; }
        public byte UserIsAdmin { get; set; }
        public string UserMail { get; set; }
        public string UserPhone { get; set; }
        public System.DateTime? UserDeletedUtc { get; set; }
    }
}