using System.ComponentModel.DataAnnotations;

namespace Restaurant.Application.DTOs.Requests
{
    public record AddUserRequest(
        [Required] string Login,
        [Required, MinLength(8)] string Password,
        [Required, EmailAddress] string Email,
        string Role = "User"
    );
}
