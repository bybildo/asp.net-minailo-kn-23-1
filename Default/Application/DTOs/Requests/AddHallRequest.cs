using System.ComponentModel.DataAnnotations;

namespace Default.Application.DTOs.Requests
{
    public record AddHallRequest(
    [Required] string Name, 
    [Required] int Width,
    [Required] int Length);
}
