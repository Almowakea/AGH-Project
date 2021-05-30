using System;
using System.Security.Cryptography;
using System.Text;
using System.Web.Helpers;

namespace AGH.Services
{
    // Service layer class
    // Uses SHA-512 algorithm

    public static class HashPasswordService
    {
        public static string CreateSalt()
        {
            // Generate unique salt for each user
            return Crypto.GenerateSalt();

        }


        public static string CreateHash(string userPassword, string salt)
        {
            string hashResult = "";

            using (SHA512 sha512Hash = SHA512.Create())
            {
                // From String to byte array + salt
                byte[] hashBytes = sha512Hash.ComputeHash(Encoding.UTF8.GetBytes(userPassword + salt));

                // Converting hashed byte array back to string format
                hashResult = BitConverter.ToString(hashBytes).Replace("-", String.Empty);
            }

            return hashResult;
        }
        

        public static bool CompareHash(string enteredPassword, string storedSalt, string storedPassword)
        {

            using (SHA512 sha512Hash = SHA512.Create())
            {
                // Append entered password with stored salt & hash them
                byte[] hashBytes = sha512Hash.ComputeHash(Encoding.UTF8.GetBytes(enteredPassword + storedSalt));

                // Converting hashed byte array back to string format
                enteredPassword = BitConverter.ToString(hashBytes).Replace("-", String.Empty);
            }

            return storedPassword.Equals(enteredPassword);
        }

    }
}