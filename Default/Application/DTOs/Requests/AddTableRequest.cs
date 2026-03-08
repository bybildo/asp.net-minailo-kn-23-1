using System.ComponentModel.DataAnnotations;

namespace Default.Application.DTOs.Requests
{
    public record AddTableRequest(
    [Required] Guid HallId,
    [Required] int Number,
    [Required] int Capacity,
    [Required] int WidthPosition,
    [Required] int LengthPosition);
}
