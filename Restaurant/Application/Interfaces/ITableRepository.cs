using Restaurant.Domain.Entities;

namespace Restaurant.Application.Interfaces
{
    public interface ITableRepository
    {
        Task<Table?> GetByIdAsync(Guid id);
        Task<List<Table>> GetAllAsync();
        Task<List<Table>> GetByHallIdAsync(Guid hallId);
        Task<List<Table>> GetByIdsAsync(List<Guid> ids);
        Task<bool> NumberExistsInHallAsync(int number, Guid hallId);
        Task<int> GetMaxWidthPositionInHallAsync(Guid hallId);
        Task<int> GetMaxLengthPositionInHallAsync(Guid hallId);
        Task AddAsync(Table table);
        void Remove(Table table);
        Task SaveChangesAsync(CancellationToken ct = default);
    }
}
