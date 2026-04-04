using Restaurant.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Restaurant.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public Role Role { get; set; }
    }
}
