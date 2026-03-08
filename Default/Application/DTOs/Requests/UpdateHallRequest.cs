using System.ComponentModel.DataAnnotations;

namespace Default.Application.DTOs.Requests
{
    public record UpdateHallRequest(
    [Required] Guid Id,
    string? Name,
    int? Width,
    int? Length
    );
}
