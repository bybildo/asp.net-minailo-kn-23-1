using Restaurant.Domain.Entities;

namespace Restaurant.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByLoginAsync(string login);
        IQueryable<User> Query();
        Task AddAsync(User user);
        void Remove(User user);
        Task<bool> ExistsAsync(Guid id, CancellationToken ct);
        Task SaveChangesAsync(CancellationToken ct = default);
    }
}
