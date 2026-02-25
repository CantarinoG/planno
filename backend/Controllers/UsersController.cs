using Backend.Models;
using Backend.Models.DTOs;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly IAuthService _authService;

        public UsersController(IUsersService usersService, IAuthService authService)
        {
            _usersService = usersService;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
        {
            if (await _usersService.GetUserByEmailAsync(request.Email) != null)
                return BadRequest("Email already in use.");

            if (await _usersService.GetUserByUsernameAsync(request.Username) != null)
                return BadRequest("Username already taken.");

            var newUser = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = _authService.HashPassword(request.Password)
            };

            await _usersService.CreateUserAsync(newUser);

            var token = _authService.GenerateJwtToken(newUser.Id!, newUser.Username, newUser.Email);

            Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = false, // Set to true in production
                SameSite = SameSiteMode.Lax,
                Expires = DateTime.UtcNow.AddDays(7)
            });

            return Ok(new AuthResponse
            {
                Token = token,
                Username = newUser.Username,
                Email = newUser.Email
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
        {
            User? user = null;

            user = await _usersService.GetUserByEmailAsync(request.EmailOrUsername);
            if (user == null)
            {
                user = await _usersService.GetUserByUsernameAsync(request.EmailOrUsername);
            }

            if (user == null || !_authService.VerifyPassword(request.Password, user.PasswordHash))
                return Unauthorized("Invalid credentials.");

            var token = _authService.GenerateJwtToken(user.Id!, user.Username, user.Email);

            Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = false, // Set to true in production
                SameSite = SameSiteMode.Lax,
                Expires = DateTime.UtcNow.AddDays(7)
            });

            return Ok(new AuthResponse
            {
                Token = token,
                Username = user.Username,
                Email = user.Email
            });
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<AuthResponse>> GetCurrentUser()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var user = await _usersService.GetUserByIdAsync(userId);
            if (user == null)
                return NotFound();

            return Ok(new AuthResponse
            {
                Token = string.Empty,
                Username = user.Username,
                Email = user.Email
            });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Append("jwt", "", new CookieOptions
            {
                HttpOnly = true,
                Secure = false, // Set to true in production
                SameSite = SameSiteMode.Lax,
                Expires = DateTime.UtcNow.AddDays(-1)
            });
            return Ok(new { message = "Logged out successfully" });
        }
    }
}
