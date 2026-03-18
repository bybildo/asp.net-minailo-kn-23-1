using System.ComponentModel.DataAnnotations;

namespace Restaurant.Application.DTOs.Requests
{
    public record AddHallRequest(
        [Required] string Name,
        [Required] int Width,
        [Required] int Length
    );
}
