using System.ComponentModel.DataAnnotations;

namespace Restaurant.Application.DTOs.Requests
{
    public record AddReservationRequest(
        Guid? UserId,
        [Required, MinLength(1)] List<Guid> TableIds,
        [Required] DateTime StartDate,
        [Required] DateTime EndDate
    )
    {
        public Guid? UserId { get; set; } = UserId;
    }
}
