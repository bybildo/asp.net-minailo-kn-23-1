using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Restaurant.Application.Identity;
using Restaurant.Application.Interfaces;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Enums;

namespace Restaurant.Infrastructure.Repositories;

public class IdentityUserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public IdentityUserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        var identityUser = await _context.Set<ApplicationUser>()
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);

        if (identityUser is null)
            return null;

        var role = await GetUserRoleAsync(identityUser.Id);
        return MapToDomainUser(identityUser, role);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        var identityUser = await _context.Set<ApplicationUser>()
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);

        if (identityUser is null)
            return null;

        var role = await GetUserRoleAsync(identityUser.Id);
        return MapToDomainUser(identityUser, role);
    }

    public IQueryable<User> Query()
    {
        return
            from user in _context.Set<ApplicationUser>().AsNoTracking()
            join userRole in _context.Set<IdentityUserRole<Guid>>() on user.Id equals userRole.UserId into userRoleGroup
            from userRole in userRoleGroup.DefaultIfEmpty()
            join role in _context.Set<IdentityRole<Guid>>() on userRole.RoleId equals role.Id into roleGroup
            from role in roleGroup.DefaultIfEmpty()
            select new User
            {
                Id = user.Id,
                Login = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                PasswordHash = user.PasswordHash ?? string.Empty,
                Role = ParseRole(role != null ? role.Name : null)
            };
    }

    public async Task AddAsync(User user)
    {
        var identityUser = new ApplicationUser
        {
            Id = user.Id,
            UserName = user.Login,
            Email = user.Email,
            PasswordHash = user.PasswordHash
        };

        await _context.Set<ApplicationUser>().AddAsync(identityUser);
    }

    public void Remove(User user)
    {
        var identityUser = _context.Set<ApplicationUser>().Local.FirstOrDefault(u => u.Id == user.Id);
        if (identityUser is null)
        {
            identityUser = new ApplicationUser { Id = user.Id };
            _context.Attach(identityUser);
        }

        _context.Set<ApplicationUser>().Remove(identityUser);
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken ct)
        => await _context.Set<ApplicationUser>().AnyAsync(u => u.Id == id, ct);

    public async Task SaveChangesAsync(CancellationToken ct = default)
        => await _context.SaveChangesAsync(ct);

    private async Task<Role> GetUserRoleAsync(Guid userId)
    {
        var roleName = await
            (from userRole in _context.Set<IdentityUserRole<Guid>>()
             join role in _context.Set<IdentityRole<Guid>>() on userRole.RoleId equals role.Id
             where userRole.UserId == userId
             select role.Name)
            .FirstOrDefaultAsync();

        return ParseRole(roleName);
    }

    private static User MapToDomainUser(ApplicationUser user, Role role)
    {
        return new User
        {
            Id = user.Id,
            Login = user.UserName ?? string.Empty,
            Email = user.Email ?? string.Empty,
            PasswordHash = user.PasswordHash ?? string.Empty,
            Role = role
        };
    }

    private static Role ParseRole(string? roleName)
    {
        if (Enum.TryParse<Role>(roleName, true, out var parsedRole))
            return parsedRole;

        return Role.User;
    }
}
