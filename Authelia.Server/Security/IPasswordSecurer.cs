namespace Authelia.Server.Security
{
    public interface IPasswordSecurer
    {
        string Secure(string clearTextPassword);
    }
}
