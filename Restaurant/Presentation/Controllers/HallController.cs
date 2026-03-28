using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.DTOs.Requests;
using Restaurant.Application.Interfaces;
using Restaurant.Domain.Entities;

namespace Restaurant.Presentation.Controllers
{
    [ApiController]
    [Route("api/halls")]
    public class HallController : ControllerBase
    {
        private readonly IHallService _hallService;

        public HallController(IHallService hallService)
        {
            _hallService = hallService ?? throw new ArgumentNullException(nameof(hallService));
        }

        [HttpGet]
        public async Task<ActionResult<List<Hall>>> GetAll()
        {
            var halls = await _hallService.GetAllHalls();
            return Ok(halls);
        }

        [HttpGet]
        [Route("find")]
        public async Task<ActionResult<Hall>> GetBySearchRequest([FromQuery] SearchHallRequest request)
        {
            var hall = await _hallService.GetHallByRequest(request);

            if (hall == null)
                return NotFound();

            return Ok(hall);
        }

        [HttpPost]
        public async Task<IActionResult> AddHall([FromBody] AddHallRequest request)
        {
            await _hallService.AddHall(request);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateHall([FromBody] UpdateHallRequest request)
        {
            await _hallService.UpdateHall(request);
            return Ok();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteHall(Guid id)
        {
            await _hallService.DeleteHall(id);
            return Ok();
        }
    }
}
