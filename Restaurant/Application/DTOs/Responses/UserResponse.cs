using Restaurant.Domain.Entities;
using Restaurant.Domain.Enums;

namespace Application.DTOs.Responses
{
    public record UserResponse
    {
        public Guid Id;
        public string Login;
        public string Email;
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
