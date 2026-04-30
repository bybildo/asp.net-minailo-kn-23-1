using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.Interfaces;
using Restaurant.Presentation.ViewModels.Reservations;
using System.Security.Claims;

namespace Restaurant.Presentation.Controllers;

public class ReservationsController : Controller
{
    private readonly IReservationService _reservationService;

    public ReservationsController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    [Authorize]
    public async Task<IActionResult> Index()
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var reservations = await _reservationService.GetAllReservationsByUserId(userId);
        return View(new ReservationsIndexViewModel { Reservations = reservations });
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _reservationService.DeleteReservation(id, Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value));
        return RedirectToAction(nameof(Index));
    }
}
