/// \file AuthController.cs
/// \brief Contains controllers for user authentication and protected access.


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Shop.Server.Services;
using Shop.Shared;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static System.Net.WebRequestMethods;

namespace Shop.Server.Controllers
{
    /// \brief Handles user registration, login, and user info retrieval.
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly IConfiguration _configuration;

        /// \brief Constructor for AuthController.
        /// \param userService The service handling user-related logic.
        /// \param configuration Application configuration for JWT.
        public AuthController(UserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        /// \brief Registers a new user.
        /// \param user User object containing username and password.
        /// \return Success or error message.
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            if (await _userService.RegisterUser(user.username, user.passwordhash))
                return Ok(new { Message = "User registered successfully" });

            return BadRequest(new { Message = "Username already exists" });
        }

        /// \brief Logs in an existing user and returns a JWT token.
        /// \param user User object with login credentials.
        /// \return JWT token and role, or error if login fails.
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            var existingUser = await _userService.GetUserByUsername(user.username);
            if (existingUser == null || !_userService.VerifyPassword(user.passwordhash, existingUser.passwordhash))
                return Unauthorized(new { Message = "Invalid username or password" });

            var token = GenerateJwtToken(existingUser);
            return Ok(new { Token = token, Role = existingUser.role });
        }

        /// \brief Generates a JWT token for the authenticated user.
        /// \param user User for whom the token is generated.
        /// \return Signed JWT token string.
        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.username),
                new Claim(ClaimTypes.Role, user.role)
            };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// \brief Retrieves authenticated user's information.
        /// \return Username and role, or error if unauthorized.
        [HttpGet("user-info")]
        [Authorize]
        public IActionResult GetUserInfo()
        {
            var username = User.Identity?.Name;
            var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(role))
            {
                return Unauthorized(new { Message = "User information is missing or invalid." });
            }

            return Ok(new { Username = username, Role = role });
        }
    }

    /// \brief Controller for testing protected access with authentication.
    [ApiController]
    [Route("api/protected")]
    public class ProtectedController : ControllerBase
    {
        /// \brief Returns data accessible only to authorized users.
        /// \return Protected data message with username.
        [Authorize]
        [HttpGet]
        public IActionResult GetProtectedData()
        {
            var username = User.Identity?.Name;
            return Ok(new { Message = "This is protected data", Username = username });
        }
    }
}
