using Restaurant.Domain.Entities;
using Restaurant.Domain.Enums;

namespace Application.DTOs.Responses
{
    public record UserResponse
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }

        public UserResponse(User user)
        {
            Id = user.Id;
            Login = user.Login;
            Email = user.Email;
            Role = user.Role;
        }
    }
}
