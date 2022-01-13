namespace Authelia.Database.Model
{
    public partial class UserDto
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public int UserOrder { get; set; }
        public System.DateTime? UserCreated { get; set; }
        public string UserCreatorIp { get; set; }
        public byte UserVerified { get; set; }
        public string UserMail { get; set; }
    }
}