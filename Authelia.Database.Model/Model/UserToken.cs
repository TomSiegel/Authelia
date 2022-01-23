using System;
using System.Collections.Generic;

#nullable disable

namespace Authelia.Database.Model
{
    public partial class UserToken
    {
        public string UserTokenId { get; set; }
        public string UserId { get; set; }
        public DateTime? TokenExpiration { get; set; }
        public DateTime? TokenCreatedUtc { get; set; }
        public string TokenCreatorIp { get; set; }

        public virtual User User { get; set; }
    }
}
