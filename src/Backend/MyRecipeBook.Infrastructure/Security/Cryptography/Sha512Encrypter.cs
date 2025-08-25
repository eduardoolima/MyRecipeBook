using MyRecipeBook.Domain.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;

namespace MyRecipeBook.Infrastructure.Security.Cryptography
{
    public class Sha512Encrypter : IPasswordEncripter
    {
        readonly string _addtionalKey;
        public Sha512Encrypter(string additionalKey)
        {
            _addtionalKey = additionalKey;
        }

        public string Encrypt(string password)
        {
            var bytes = Encoding.UTF8.GetBytes($"{password}-{_addtionalKey}");
            var hashBytes = SHA512.HashData(bytes);
            return StringBytes(hashBytes);
        }

        static string StringBytes(byte[] bytes)
        {
            StringBuilder sb = new();
            foreach (var item in bytes)
            {
                var hex = item.ToString("x2");
                sb.Append(hex);
            }
            return sb.ToString();
        }
    }
}
