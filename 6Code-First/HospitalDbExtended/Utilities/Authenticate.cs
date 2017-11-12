using System;
using System.Security.Cryptography;
using System.Text;

namespace HospitalDbExtended.Utilities
{
    public static class Authenticate
    {
        public static string GetHash(string password)
        {
            // SHA256 is disposable by inheritance.  
            using (SHA256 sha256 = SHA256.Create())
            {
                // Send a sample password to hash.  
                byte[] hashedBytes = sha256.ComputeHash(Encoding.Unicode.GetBytes(password));

                // Get the hashed string.  
                return BitConverter.ToString(hashedBytes).Replace("-", string.Empty).ToLower();
            }
        }

        public static string GetSalt()
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