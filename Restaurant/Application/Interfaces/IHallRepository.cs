using Restaurant.Domain.Entities;

namespace Restaurant.Application.Interfaces
{
    public interface IHallRepository
    {
        Task<Hall?> GetByIdAsync(Guid id);
        Task<List<Hall>> GetAllAsync();
        IQueryable<Hall> Query();
        Task AddAsync(Hall hall);
        void Update(Hall hall);
        void Remove(Hall hall);
        Task SaveChangesAsync(CancellationToken ct = default);
    }
}
