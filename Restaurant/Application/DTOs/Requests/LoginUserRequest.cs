using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Requests
{
    public record LoginUserRequest(
        [Required, EmailAddress] string Email,
        [Required] string Password
    );
}
