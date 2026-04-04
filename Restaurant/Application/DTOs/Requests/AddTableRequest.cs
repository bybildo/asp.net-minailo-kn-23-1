using System.ComponentModel.DataAnnotations;

namespace Restaurant.Application.DTOs.Requests
{
    public record AddTableRequest(
        [Required] Guid HallId,
        [Required] int Number,
        [Required] int Capacity,
        [Required] int WidthPosition,
        [Required] int LengthPosition
    );
}
