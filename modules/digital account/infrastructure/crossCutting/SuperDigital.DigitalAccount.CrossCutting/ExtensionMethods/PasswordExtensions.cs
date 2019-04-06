using System;
using System.Security.Cryptography;
using System.Text;

namespace SuperDigital.DigitalAccount.CrossCutting.ExtensionMethods
{
    public static class PasswordExtensions
    {
        public static string GeneratePassword(this string password, string passwordSalt)
        {
            string inputString = $"{password}{passwordSalt}";
            return GenerateSHA256String(inputString);
        }

        public static string GenerateSHA1(this string value)
        {
            try
            {
                byte[] buffer = Encoding.Default.GetBytes(value);
                System.Security.Cryptography.SHA1CryptoServiceProvider cryptoTransformSHA1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();
                string hash = BitConverter.ToString(cryptoTransformSHA1.ComputeHash(buffer)).Replace("-", "");
                return hash.ToLower();
            }
            catch (Exception x)
            {
                throw new Exception(x.Message);
            }
        }

        private static string GenerateSHA256String(string inputString)
        {
            SHA256 sha256 = SHA256.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(inputString);
            byte[] hash = sha256.ComputeHash(bytes);
            return GetStringFromHash(hash);
        }

        private static string GenerateSHA512String(string inputString)
        {
            SHA512 sha512 = SHA512.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(inputString);
            byte[] hash = sha512.ComputeHash(bytes);
            return GetStringFromHash(hash);
        }

        private static string GetStringFromHash(byte[] hash)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }
    }
}
