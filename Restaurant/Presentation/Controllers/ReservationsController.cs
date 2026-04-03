using Microsoft.AspNetCore.Mvc;

namespace Restaurant.Presentation.Controllers;

public class ReservationsController : Controller
{
    public IActionResult Index() => View();

    public IActionResult Create() => View();
}
