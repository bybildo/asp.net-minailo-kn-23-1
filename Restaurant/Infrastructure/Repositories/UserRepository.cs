using Microsoft.EntityFrameworkCore;
using Restaurant.Application.Interfaces;
using Restaurant.Domain.Entities;

namespace Restaurant.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByIdAsync(Guid id)
            => await _context.Users.FindAsync(id);

        public async Task<User?> GetByEmailAsync(string email)
            => await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        public IQueryable<User> Query()
            => _context.Users.AsQueryable();

        public async Task AddAsync(User user)
            => await _context.Users.AddAsync(user);

        public void Remove(User user)
            => _context.Users.Remove(user);

        public async Task<bool> ExistsAsync(Guid id, CancellationToken ct)
            => await _context.Users.AnyAsync(u => u.Id == id, ct);

        public async Task SaveChangesAsync(CancellationToken ct = default)
            => await _context.SaveChangesAsync(ct);
    }
}
