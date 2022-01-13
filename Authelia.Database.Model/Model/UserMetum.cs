using System;
using System.Collections.Generic;

#nullable disable

namespace Authelia.Database.Model
{
    public partial class UserMetum
    {
        public string UserId { get; set; }
        public string UserMetaKey { get; set; }
        public string UserMetaValue { get; set; }
    }
}
