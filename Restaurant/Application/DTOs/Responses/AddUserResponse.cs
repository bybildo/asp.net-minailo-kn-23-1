namespace Restaurant.Application.DTOs.Responses
{
    public record AddUserResponse(
        string Token,
        string Role
    );
}
