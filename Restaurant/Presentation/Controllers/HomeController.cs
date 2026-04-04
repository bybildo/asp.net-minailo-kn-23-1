using Microsoft.AspNetCore.Mvc;

namespace Restaurant.Presentation.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
