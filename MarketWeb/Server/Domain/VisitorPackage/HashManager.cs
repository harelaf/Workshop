using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;

namespace MarketWeb.Server.Domain
{
    internal static class HashManager
    {
        private static readonly RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();

        /// <summary>
        /// Generate a 128-bit salt using a cryptographically strong random sequence of nonzero values
        /// </summary>
        /// <returns></returns>
        public static string GenerateSalt()
        {
            byte[] salt = new byte[128 / 8];
            randomNumberGenerator.GetNonZeroBytes(salt);
            return Convert.ToBase64String(salt);
        }

        /// <summary>
        /// <see href="https://docs.microsoft.com/en-us/aspnet/core/security/data-protection/consumer-apis/password-hashing?view=aspnetcore-6.0">Article on topic</see>
        /// </summary>
        /// <param name="cleartext"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string Hash(string cleartext, string salt = "")
        {
            byte[] byteSalt = Convert.FromBase64String(salt);
            // derive a 256 - bit subkey(use HMACSHA256 with 100, 000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: cleartext,
            salt: byteSalt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));
            return hashed;
        }

        public static bool CompareCleartextToHash(string cleartext, string salt, string hash)
        {
            string hashedCleartext = Hash(cleartext, salt);
            return hashedCleartext.Equals(hash);
        }
    }
}
