using Default.Application.DTOs.Requests;
using Default.Application.Services;
using Default.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;

namespace Default.Controllers
{
    [ApiController]
    [Route("hall")]
    public class HallController : ControllerBase
    {
        private readonly HallService _hallService;

        public HallController(HallService hallService)
        {
            _hallService = hallService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Hall>>> GetAll()
        {
            var halls = await _hallService.GetAllHalls();
            return Ok(halls);
        }

        [HttpGet("find")]
        public async Task<ActionResult<Hall>> GetByRequest([FromQuery] SearchHallRequest request)
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHall(Guid id)
        {
            await _hallService.DeleteHall(id);
            return Ok();
        }
    }
}
