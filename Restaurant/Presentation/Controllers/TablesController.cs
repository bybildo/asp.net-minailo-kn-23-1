using Microsoft.AspNetCore.Mvc;

namespace Restaurant.Presentation.Controllers;

public class TablesController : Controller
{
    public IActionResult Index() => View();
}
