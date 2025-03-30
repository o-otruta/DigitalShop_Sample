/// \file UserService.cs
/// \brief Provides user management functionality such as registration, authentication, and password hashing.


using Microsoft.EntityFrameworkCore;
using Shop.Server.Data;
using Shop.Shared;
using System.Security.Cryptography;
using System.Text;

namespace Shop.Server.Services
{
    /// \brief Service for user-related operations such as registration and login verification.
    public class UserService
    {
        private readonly AppDbContext _context;

        /// \brief Constructor for UserService.
        /// \param context The application's database context.
        public UserService(AppDbContext context)
        {
            _context = context;
        }

        /// \brief Retrieves a user by username.
        /// \param username The username to search for.
        /// \return The User object or null if not found.
        public async Task<User?> GetUserByUsername(string username)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.username == username);
        }

        /// \brief Registers a new user if the username is not already taken.
        /// \param username The username for the new user.
        /// \param password The plaintext password.
        /// \return True if registration is successful, false if username already exists.
        public async Task<bool> RegisterUser(string username, string password)
        {
            if (await GetUserByUsername(username) != null)
                return false;

            var passwordHash = HashPassword(password);
            var user = new User
            {
                username = username,
                passwordhash = passwordHash
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }

        /// \brief Verifies whether the provided password matches the stored hash.
        /// \param password The plaintext password to verify.
        /// \param passwordHash The stored hashed password.
        /// \return True if the password is correct; otherwise, false.
        public bool VerifyPassword(string password, string passwordHash)
        {
            return HashPassword(password) == passwordHash;
        }

        /// \brief Hashes a password using SHA-256 algorithm.
        /// \param password The plaintext password to hash.
        /// \return The Base64-encoded hashed password.
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}
