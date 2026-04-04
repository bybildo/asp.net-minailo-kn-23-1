using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Restaurant.Presentation.Controllers;

public class AdminController : Controller
{
    [Authorize(Roles = "Admin")]
    public IActionResult Index() => View();
}
