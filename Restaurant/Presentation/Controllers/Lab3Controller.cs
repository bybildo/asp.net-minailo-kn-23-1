using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.DTOs.Requests;
using Restaurant.Application.Interfaces;
using Restaurant.Presentation.Filters;
using Restaurant.Presentation.ViewModels.Lab3;

namespace Restaurant.Presentation.Controllers;

[Authorize(Roles = "Admin")]
[ControllerLevelLogFilter]
public class Lab3Controller : Controller
{
    private readonly IHallService _hallService;

    public Lab3Controller(IHallService hallService)
    {
        _hallService = hallService;
    }

    public async Task<IActionResult> Index()
    {
        var halls = await _hallService.GetAllHalls();
        var model = halls.Select(h => new HallListItemViewModel
        {
            Id = h.Id,
            Name = h.Name,
            Width = h.Width,
            Length = h.Length
        }).ToList();

        return View(model);
    }

    [HttpGet]
    [TypeFilter(typeof(TypeLevelLogFilter), Arguments = new object[] { "Lab3EditGet" })]
    public async Task<IActionResult> Edit(Guid id)
    {
        var hall = await _hallService.GetHallByRequest(new SearchHallRequest(Id: id, Name: null));
        if (hall is null)
            return NotFound();

        return View(new HallEditViewModel
        {
            Id = hall.Id,
            Name = hall.Name,
            Width = hall.Width,
            Length = hall.Length
        });
    }

    [HttpPost]
    [ActionLevelLogFilter]
    public async Task<IActionResult> Edit(HallEditViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        await _hallService.UpdateHall(new UpdateHallRequest(
            model.Id,
            model.Name,
            model.Width,
            model.Length
        ));

        return RedirectToAction(nameof(Index));
    }
}
