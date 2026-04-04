using Microsoft.AspNetCore.Mvc;

namespace Restaurant.Presentation.Controllers;

public class AccountController : Controller
{
    public IActionResult Login() => View();

    public IActionResult Register() => View();
}
