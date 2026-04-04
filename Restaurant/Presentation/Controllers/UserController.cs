using Application.DTOs.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.DTOs.Requests;
using Restaurant.Application.Interfaces;
using System.Security.Claims;

namespace Restaurant.Presentation.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [HttpGet("verify")]
        [Authorize]
        public async Task<IActionResult> VerifyAuth()
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var login = User.FindFirst(ClaimTypes.Name)!.Value;
            var role = User.FindFirst(ClaimTypes.Role)!.Value;
            return Ok(new { id, login, role });
        }

        [HttpGet("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete("jwt_token");
            return Ok(new { message = "Logged out successfully" });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AddUserRequest request)
        {
            var authResponse = await _userService.Register(request);

            Response.Cookies.Append("jwt_token", authResponse.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(30)
            }); 

            return Ok(new { message = "Registered successfully" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
        {
            var authResponse = await _userService.Login(request);

            Response.Cookies.Append("jwt_token", authResponse.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(30)
            });

            return Ok(new { message = "Logged in successfully" });
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] SearchUserRequest request)
        {
            var users = await _userService.GetUsersByRequest(request);
            return Ok(users);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddUser([FromBody] AddUserRequest request)
        {
            await _userService.AddUser(request);
            return Ok(new { message = "User created successfully" });
        }

        [HttpGet("find")]
        public async Task<IActionResult> GetUser([FromQuery] SearchUserRequest request)
        {
            var user = await _userService.GetUserByRequest(request);
            return Ok(user);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            await _userService.DeleteUser(id);
            return Ok(new { message = "User deleted successfully" });
        }
    }
}
