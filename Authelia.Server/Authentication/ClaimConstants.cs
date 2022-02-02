using System.Security.Claims;

namespace Authelia.Server.Authentication
{
    public class ClaimConstants
    {
        public const string Verified = "Verified";
        public const string Username = ClaimTypes.Name;
        public const string UserIdentifier = ClaimTypes.NameIdentifier;
        public const string Email = ClaimTypes.Email;
        public const string Phone = ClaimTypes.MobilePhone;
        public const string Role = ClaimTypes.Role;
    }
}
