using Microsoft.EntityFrameworkCore;
using Shop.Server.Data;
using Shop.Shared;
using System.Security.Cryptography;
using System.Text;

namespace Shop.Server.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByUsername(string username)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.username == username);
        }

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

        public bool VerifyPassword(string password, string passwordHash)
        {
            return HashPassword(password) == passwordHash;
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}
