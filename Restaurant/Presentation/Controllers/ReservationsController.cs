using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.Interfaces;

namespace Restaurant.Presentation.Controllers;

public class ReservationsController : Controller
{
    [Authorize]
    public IActionResult Index() => View();
}
