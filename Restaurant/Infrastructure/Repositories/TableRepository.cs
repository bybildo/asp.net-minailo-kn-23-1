using Microsoft.EntityFrameworkCore;
using Restaurant.Application.Interfaces;
using Restaurant.Domain.Entities;

namespace Restaurant.Infrastructure.Repositories
{
    public class TableRepository : ITableRepository
    {
        private readonly AppDbContext _context;

        public TableRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Table?> GetByIdAsync(Guid id)
            => await _context.Tables.FindAsync(id);

        public async Task<List<Table>> GetAllAsync()
            => await _context.Tables.ToListAsync();

        public async Task<List<Table>> GetByHallIdAsync(Guid hallId)
            => await _context.Tables.Where(t => t.HallId == hallId).ToListAsync();

        public async Task<List<Table>> GetByIdsAsync(List<Guid> ids)
            => await _context.Tables.Where(t => ids.Contains(t.Id)).ToListAsync();

        public async Task<bool> NumberExistsInHallAsync(int number, Guid hallId)
            => await _context.Tables.AnyAsync(t => t.Number == number && t.HallId == hallId);

        public async Task<bool> PositionExistsInHallAsync(int widthPosition, int lengthPosition, Guid hallId)
            => await _context.Tables.Where(t => t.HallId == hallId).AnyAsync(t => t.WidthPosition == widthPosition && t.LengthPosition == lengthPosition) ? true : false;

        public async Task<int> GetMaxWidthPositionInHallAsync(Guid hallId)
            => await _context.Tables.Where(t => t.HallId == hallId).AnyAsync()
                ? await _context.Tables.Where(t => t.HallId == hallId).MaxAsync(t => t.WidthPosition)
                : 0;

        public async Task<int> GetMaxLengthPositionInHallAsync(Guid hallId)
            => await _context.Tables.Where(t => t.HallId == hallId).AnyAsync()
                ? await _context.Tables.Where(t => t.HallId == hallId).MaxAsync(t => t.LengthPosition)
                : 0;

        public async Task AddAsync(Table table)
            => await _context.Tables.AddAsync(table);

        public void Remove(Table table)
            => _context.Tables.Remove(table);

        public async Task SaveChangesAsync(CancellationToken ct = default)
            => await _context.SaveChangesAsync(ct);
    }
}
