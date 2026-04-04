using System.Security.Claims;

namespace Restaurant.Application.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(Guid userId, string login, string role);
        ClaimsPrincipal? ValidateToken(string token);
    }
}
