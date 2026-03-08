using Default.Models.Enum;

namespace Default.Application.DTOs.Requests
{
    public record SearchUserRequest(
      Guid? Id = null,
      string? Login = null,
      string? Email = null,
      string? Role = "User"
    );
}
