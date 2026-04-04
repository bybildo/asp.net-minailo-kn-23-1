namespace Restaurant.Application.DTOs.Requests
{
    public record SearchUserRequest(
        Guid? Id = null,
        string? Login = null,
        string? Email = null,
        string? Role = null
    );
}
