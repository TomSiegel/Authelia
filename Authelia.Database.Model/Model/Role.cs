using System;
using System.Collections.Generic;

#nullable disable

namespace Authelia.Database.Model
{
    public partial class Role
    {
        public Role()
        {
            UserRoles = new HashSet<UserRole>();
        }

        public string RoleId { get; set; }
        public string RoleName { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
