using Microsoft.EntityFrameworkCore;
using Restaurant.Application.Interfaces;
using Restaurant.Domain.Entities;

namespace Restaurant.Infrastructure.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly AppDbContext _context;

        public ReservationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Reservation?> GetByIdAsync(Guid id)
            => await _context.Reservations.FindAsync(id);

        public async Task<Reservation?> GetByIdWithTablesAsync(Guid id)
            => await _context.Reservations
                .Include(r => r.Tables)
                .FirstOrDefaultAsync(r => r.Id == id);

        public async Task<List<Reservation>> GetAllWithDetailsAsync()
            => await _context.Reservations
                .Include(r => r.Tables)
                .Include(r => r.User)
                .ToListAsync();

        public async Task<bool> HasConflictAsync(DateTime startDate, DateTime endDate, List<Guid> tableIds)
            => await _context.Reservations
                .Where(r => r.StartDate < endDate && r.EndDate > startDate)
                .AnyAsync(r => r.Tables.Any(t => tableIds.Contains(t.Id)));

        public async Task AddAsync(Reservation reservation)
            => await _context.Reservations.AddAsync(reservation);

        public void Remove(Reservation reservation)
            => _context.Reservations.Remove(reservation);

        public async Task<List<Reservation>> GetAllReservationsByUserIdAsync(Guid userId)
            => await _context.Reservations
                .Where(r => r.UserId == userId)
                .Include(r => r.Tables)
                .ThenInclude(t => t.Hall)
                .ToListAsync();

        public async Task<List<Reservation>> GetReservationsByHallId(Guid id) 
            => await _context.Reservations
                .Where(r => r.Tables.Any(t => t.HallId == id))
                .Include(r => r.Tables)
                .ThenInclude(t => t.Hall)
                .ToListAsync();

        public async Task SaveChangesAsync(CancellationToken ct = default)
            => await _context.SaveChangesAsync(ct);
    }
}
