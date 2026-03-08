namespace Default.Application.DTOs.Requests
{
    public record SearchHallRequest(
        Guid? Id,
        string? Name
    );
}
