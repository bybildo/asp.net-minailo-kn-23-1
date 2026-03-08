using Default.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace Default.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public Role Role { get; set; }
    }
}
