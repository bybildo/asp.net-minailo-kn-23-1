using Microsoft.AspNetCore.Mvc;

namespace Restaurant.Presentation.Controllers;

public class AdminController : Controller
{
    public IActionResult Index() => View();
}
