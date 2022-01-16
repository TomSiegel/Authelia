using System;
using System.Collections.Generic;

#nullable disable

namespace Authelia.Database.Model
{
    public partial class User
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public int UserOrder { get; set; }
        public DateTime? UserCreatedUtc { get; set; }
        public string UserCreatorIp { get; set; }
        public byte UserVerified { get; set; }
        public string UserMail { get; set; }
        public string UserPhone { get; set; }
        public DateTime? UserDeletedUtc { get; set; }
    }
}
