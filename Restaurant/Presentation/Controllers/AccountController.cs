using Application.DTOs.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.DTOs.Requests;
using Restaurant.Application.Interfaces;

namespace Restaurant.Presentation.Controllers;

public class AccountController : Controller
{
    private readonly IUserService _userService;

    public AccountController(IUserService userService)
    {
        _userService = userService;
    }

    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginUserRequest request)
    {
        if (!ModelState.IsValid)
            return View(request);

        var authResponse = await _userService.Login(request);
        Response.Cookies.Append("jwt_token", authResponse.Token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddDays(30)
        });

        return RedirectToAction("Index", "Home");
    }

    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(AddUserRequest request)
    {
        if (!ModelState.IsValid)
            return View(request);

        var authResponse = await _userService.Register(request);
        Response.Cookies.Append("jwt_token", authResponse.Token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddDays(30)
        });

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [Authorize]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("jwt_token");
        return RedirectToAction("Index", "Home");
    }
}
