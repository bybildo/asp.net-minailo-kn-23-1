using Microsoft.AspNetCore.Mvc;
using Application.DTOs.Responses;
using Restaurant.Application.DTOs.Requests;
using Restaurant.Application.Interfaces;
using Restaurant.Presentation.ViewModels.Tables;
using System.Security.Claims;

namespace Restaurant.Presentation.Controllers;

public class TablesController : Controller
{
    private readonly IHallService _hallService;
    private readonly ITableService _tableService;
    private readonly IReservationService _reservationService;

    public TablesController(IHallService hallService, ITableService tableService, IReservationService reservationService)
    {
        _hallService = hallService;
        _tableService = tableService;
        _reservationService = reservationService;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.Identity?.IsAuthenticated == true
            ? Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value)
            : (Guid?)null;

        return View(new TablesIndexViewModel
        {
            UserId = userId,
            Halls = await _hallService.GetAllHalls(),
            Tables = await _tableService.GetAllTables(),
            Reservations = (await _reservationService.GetAllReservations()).Select(r => new ReservationResponse(r)).ToList()
        });
    }

    [HttpPost]
    public async Task<IActionResult> Book(List<Guid> tableIds, DateTime startDate, DateTime endDate)
    {
        if (User.Identity?.IsAuthenticated != true)
            return RedirectToAction("Login", "Account");

        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await _reservationService.AddReservation(new AddReservationRequest(userId, tableIds, startDate, endDate));
        return RedirectToAction("Index", "Reservations");
    }
}
