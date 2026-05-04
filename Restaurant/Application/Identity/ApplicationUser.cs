using Microsoft.AspNetCore.Identity;

namespace Restaurant.Application.Identity;

public class ApplicationUser : IdentityUser<Guid>
{
    public string FullName { get; set; } = string.Empty;
}
