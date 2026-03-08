using Default.Application.DTOs.Requests;
using Default.Application.Services;
using Default.Models;
using Microsoft.AspNetCore.Mvc;

namespace Default.Controllers
{
    [ApiController]
    [Route("reservation")]
    public class ReservationController : ControllerBase
    {
        private readonly ReservationService _reservationService;

        public ReservationController(ReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Reservation>>> GetAll()
        {
            var reservations = await _reservationService.GetAllReservations();
            return Ok(reservations);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Reservation>> GetById(Guid id)
        {
            var reservation = await _reservationService.GetReservationById(id);
            if (reservation == null)
                return NotFound();

            return Ok(reservation);
        }

        [HttpPost]
        [JwtAuth]
        public async Task<IActionResult> Add([FromBody] AddReservationRequest request)
        {
            var userId = (Guid)HttpContext.Items["UserId"];
            if (request.UserId == null)
            {
                if (userId != null)
                    request.UserId = userId;
                else
                    return Unauthorized();
            }

            await _reservationService.AddReservation(request);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateReservationRequest request)
        {
            await _reservationService.UpdateReservation(request);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _reservationService.DeleteReservation(id);
            return Ok();
        }
    }
}
