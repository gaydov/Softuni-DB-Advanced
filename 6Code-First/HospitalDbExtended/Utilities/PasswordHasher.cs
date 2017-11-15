using System;
using System.Security.Cryptography;
using System.Text;

namespace HospitalDbExtended.Utilities
{
    public static class PasswordHasher
    {
        public static string GenerateHash(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.Unicode.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", string.Empty).ToLower();
            }
        }

        public static string GenerateSalt()
        {
            byte[] bytes = new byte[128 / 8];

            using (RandomNumberGenerator keyGenerator = RandomNumberGenerator.Create())
            {
                keyGenerator.GetBytes(bytes);
                return BitConverter.ToString(bytes).Replace("-", string.Empty).ToLower();
            }
        }
    }
}