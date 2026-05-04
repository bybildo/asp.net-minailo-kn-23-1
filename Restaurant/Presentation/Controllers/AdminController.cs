using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs.Responses;
using Restaurant.Application.DTOs.Requests;
using Restaurant.Application.Identity;
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
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    public AdminController(
        IHallService hallService,
        ITableService tableService,
        IReservationService reservationService,
        IUserService userService,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole<Guid>> roleManager)
    {
        _hallService = hallService;
        _tableService = tableService;
        _reservationService = reservationService;
        _userService = userService;
        _userManager = userManager;
        _roleManager = roleManager;
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

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> IdentityUsers()
    {
        var users = _userManager.Users.ToList();
        var model = new IdentityUsersViewModel();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            model.Users.Add(new IdentityUserItemViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email ?? string.Empty,
                Role = roles.FirstOrDefault() ?? "User"
            });
        }

        return View(model);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeIdentityUserRole(Guid userId, string role)
    {
        if (!await _roleManager.RoleExistsAsync(role))
        {
            return RedirectToAction(nameof(IdentityUsers));
        }

        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            return RedirectToAction(nameof(IdentityUsers));
        }

        var currentRoles = await _userManager.GetRolesAsync(user);
        if (currentRoles.Count > 0)
        {
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
        }

        await _userManager.AddToRoleAsync(user, role);
        return RedirectToAction(nameof(IdentityUsers));
    }
}
