using System.Security.Claims;

namespace Authelia.Server.Authentication
{
    public class ClaimConstants
    {
        public const string Verified = "Verified";
        public const string Username = ClaimTypes.NameIdentifier;
        public const string Email = ClaimTypes.Email;
        public const string Phone = ClaimTypes.MobilePhone;
    }
}
