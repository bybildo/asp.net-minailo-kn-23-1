using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs.Responses;
using Restaurant.Application.DTOs.Requests;
using Restaurant.Application.Interfaces;
using Restaurant.Presentation.Filters;
using Restaurant.Presentation.ViewModels.Admin;

namespace Restaurant.Presentation.Controllers;

public class AdminController : Controller
{
    private readonly IHallService _hallService;
    private readonly ITableService _tableService;
    private readonly IReservationService _reservationService;
    private readonly IUserService _userService;

    public AdminController(IHallService hallService, ITableService tableService, IReservationService reservationService, IUserService userService)
    {
        _hallService = hallService;
        _tableService = tableService;
        _reservationService = reservationService;
        _userService = userService;
    }

    [Authorize(Roles = "Admin")]
    [ServiceFilter(typeof(ServiceLevelLogFilter))]
    public async Task<IActionResult> Index()
    {
        return View(new AdminIndexViewModel
        {
            Halls = await _hallService.GetAllHalls(),
            Tables = await _tableService.GetAllTables(),
            Reservations = (await _reservationService.GetAllReservations()).Select(r => new ReservationResponse(r)).ToList(),
            Users = await _userService.GetUsersByRequest(new SearchUserRequest())
        });
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddHall(AddHallRequest request)
    {
        await _hallService.AddHall(request);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateHall(UpdateHallRequest request)
    {
        await _hallService.UpdateHall(request);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteHall(Guid id)
    {
        await _hallService.DeleteHall(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddTable(AddTableRequest request)
    {
        await _tableService.AddTable(request);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteTable(Guid id)
    {
        await _tableService.DeleteTable(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddReservation(AddReservationRequest request)
    {
        await _reservationService.AddReservation(request);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteReservation(Guid id)
    {
        await _reservationService.DeleteReservation(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddUser(AddUserRequest request)
    {
        await _userService.AddUser(request);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        await _userService.DeleteUser(id);
        return RedirectToAction(nameof(Index));
    }
}
