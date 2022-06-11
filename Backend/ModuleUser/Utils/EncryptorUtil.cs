using System.Security.Cryptography;
using System.Text;

namespace ModuleUser.Utils
{
    public class EncryptorUtil
    {
        private readonly HashAlgorithm hash;

        public EncryptorUtil(HashAlgorithm hash)
        {
            this.hash = hash;
        }

        public string Hash(string content)
        {
            var hashValue = hash.ComputeHash(Encoding.Unicode.GetBytes(content));
            var hashString = Convert.ToHexString(hashValue);
            return hashString;
        }
    }
}