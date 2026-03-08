using Default.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace Default.Application.DTOs.Requests
{
    public record UpdateReservationRequest(
       [Required] Guid ReservationId,
       [Required] Guid TableId,
       [Required] string Operation = "Add"
       );
}
