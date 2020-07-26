using System;
using JWT.Models;
using Microsoft.AspNetCore.Identity;

namespace JWT.Extensions
{
    public class HasherExtension
    {
        public static string HashPassword(string password)
        {
            var hasher = new PasswordHasher<User>();
            var hashPassword = hasher.HashPassword(null, password);

            return hashPassword;
        }

        public static bool VerifyHashedPassword(string passwordHash, string providedPassword)
        {
            var hasher = new PasswordHasher<User>();
            var verify = hasher.VerifyHashedPassword(null, passwordHash, providedPassword);

            if (verify == PasswordVerificationResult.Success)
            {
                return true;
            }

            return false;
        }
    }
}
