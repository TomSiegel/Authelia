namespace Authelia.Server.Requests.Entities
{
    public class AdminSetupResponse
    {
        public int Count { get; set; }

        public bool HasAdmins { get { return Count > 0; } }
    }
}
