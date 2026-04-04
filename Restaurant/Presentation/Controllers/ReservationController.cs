using Application.DTOs.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.DTOs.Requests;
using Restaurant.Application.Interfaces;
using Restaurant.Domain.Entities;
using System.Security.Claims;

namespace Restaurant.Presentation.Controllers
{
    [ApiController]
    [Route("api/reservations")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService ?? throw new ArgumentNullException(nameof(reservationService));
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<ReservationResponse>>> GetReservations()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var reservations = await _reservationService.GetAllReservationsByUserId(userId);

            return Ok(reservations);
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<Reservation>>> GetAllReservations()
        {
            var reservations = await _reservationService.GetAllReservations();
            return Ok(reservations);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Reservation>> GetById(Guid id)
        {
            var reservation = await _reservationService.GetReservationById(id);
            if (reservation == null)
                return NotFound();

            return Ok(reservation);
        }

        [HttpGet("hall/{id:guid}")]
        [Authorize]
        public async Task<ActionResult<List<ReservationResponse>>> GetAllReservationsByHallId(Guid id)
        {
            var reservation = await _reservationService.GetAllReservationsByHallId(id);
            if (reservation == null)
                return NotFound();

            return Ok(reservation);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add([FromBody] AddReservationRequest request)
        {
            var roleClaim = HttpContext.User.FindFirst(ClaimTypes.Role);

            if (roleClaim?.Value == "Admin")
            {
                await _reservationService.AddReservation(request);
                return Ok();    
            }

            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = Guid.Parse(userIdClaim!.Value);

            if (request.UserId == null)
                request.UserId = userId;

            await _reservationService.AddReservation(request);
            return Ok();
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] UpdateReservationRequest request)
        {
            await _reservationService.UpdateReservation(request);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            var roleClaim = HttpContext.User.FindFirst(ClaimTypes.Role);

            if (roleClaim?.Value == "Admin")
            {
                await _reservationService.DeleteReservation(id);
                return Ok();
            }

            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            await _reservationService.DeleteReservation(id, Guid.Parse(userIdClaim!.Value));

            return Ok();
        }
    }
}
