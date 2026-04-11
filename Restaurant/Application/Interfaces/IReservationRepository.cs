using Restaurant.Domain.Entities;

namespace Restaurant.Application.Interfaces
{
    public interface IReservationRepository
    {
        Task<Reservation?> GetByIdAsync(Guid id);
        Task<Reservation?> GetByIdWithTablesAsync(Guid id);
        Task<List<Reservation>> GetAllWithDetailsAsync();
        Task<bool> HasConflictAsync(DateTime startDate, DateTime endDate, List<Guid> tableIds);
        Task AddAsync(Reservation reservation);
        void Remove(Reservation reservation);
        Task SaveChangesAsync(CancellationToken ct = default);
    }
}
