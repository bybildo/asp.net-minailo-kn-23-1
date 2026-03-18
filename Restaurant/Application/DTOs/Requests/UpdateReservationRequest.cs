using System.ComponentModel.DataAnnotations;

namespace Restaurant.Application.DTOs.Requests
{
    public record UpdateReservationRequest(
        [Required] Guid ReservationId,
        [Required] Guid TableId,
        [Required] string Operation = "Add"
    );
}
