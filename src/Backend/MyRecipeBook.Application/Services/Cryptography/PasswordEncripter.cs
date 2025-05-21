using System.Security.Cryptography;
using System.Text;

namespace MyRecipeBook.Application.Services.Cryptography
{
    public class PasswordEncripter
    {
        readonly string _addtionalKey;
        public PasswordEncripter(string additionalKey)
        {
            _addtionalKey = additionalKey;
        }

        public string Encrypt(string password)
        {
            var newPassword = $"{password}-{_addtionalKey}";
            var bytes = Encoding.UTF8.GetBytes(password);
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
