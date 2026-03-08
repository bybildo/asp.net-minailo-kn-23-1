using System.ComponentModel.DataAnnotations;

namespace Default.Application.DTOs.Requests
{
    public record AddUserRequest(
         [Required] string Login,
         [Required] string Password,
         [Required, EmailAddress] string Email,
         string Role = "User"
     );
}
