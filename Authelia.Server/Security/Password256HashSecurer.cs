using System.Security.Cryptography;
using System.Text;
using Authelia.Server.Helpers;

namespace Authelia.Server.Security
{
    public class Password256HashSecurer : IPasswordSecurer
    {
        private const string preSalt = "$Auth";
        private const string postSalt = "elia!";

        public string Secure(string clearTextPassword)
        {
            string fullClearText = preSalt + clearTextPassword + postSalt;
            string result;

            using (SHA256 mySHA256 = SHA256.Create())
            {        
                byte[] buffer = Encoding.UTF8.GetBytes(fullClearText);
                byte[] hashValue = mySHA256.ComputeHash(buffer);

                result = Binary.ToHex(hashValue);
            }

            return result;
        }
    }
}
