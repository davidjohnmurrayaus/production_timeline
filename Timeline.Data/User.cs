using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Timeline.Data
{
    /// <summary>
    /// Object to represent a user in the system.
    /// </summary>
    public class User
    {
        public int UserId;
        public string Username;
        public string PasswordHash;
        public string Name;

        /// <summary>
        /// Create a hash of the password.
        /// Algorithm is from:
        /// https://stackoverflow.com/questions/4181198/how-to-hash-a-password#10402129
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string HashPassword(string password)
        {
            // Generate salt
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            // Perform hashing
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            // Combine salt and hash
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            // Convert to string
            string savedPasswordHash = Convert.ToBase64String(hashBytes);
            return savedPasswordHash;
        }

        /// <summary>
        /// Check a password against the stored hash of the password.
        /// Algorithm is from:
        /// https://stackoverflow.com/questions/4181198/how-to-hash-a-password#10402129
        /// </summary>
        /// <param name="password"></param>
        /// <param name="saltAndHash"></param>
        public static bool VerifyHash(string password, string saltAndHash)
        {
            // Extract the bytes
            byte[] hashBytes = Convert.FromBase64String(saltAndHash);

            // Get the salt
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            // Compute the hash on the password the user entered
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            // Compare the results
            for (int i = 0; i < 20; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Attempt to login, return user object is successful.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static User Login(string username, string password)
        {
            var userDict = FindAll();
            var user = userDict[username];

            if (VerifyHash(password, user.PasswordHash))
            {
                return user;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Find all users in the system.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, User> FindAll()
        {
            var users = new Dictionary<string, User>();
            users.Add("davidm", new User() {
                UserId = 1,
                Username = "davidm",
                PasswordHash = HashPassword("password"),
                Name = "David Murray"
            });
            return users;
        }
    }
}
