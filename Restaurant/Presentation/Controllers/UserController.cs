using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.DTOs.Requests;
using Restaurant.Application.Interfaces;

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

        [HttpPost("auth")]
        public async Task<IActionResult> RegisterOrLogin([FromBody] AddUserRequest request)
        {
            var authResponse = await _userService.RegisterOrLogin(request);

            Response.Cookies.Append("jwt_token", authResponse.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(30)
            });

            return Ok(new { message = "Authenticated successfully", role = authResponse.Role });
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] SearchUserRequest request)
        {
            var users = await _userService.GetUsersByRequest(request);
            return Ok(users);
        }

        [HttpPost]
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
