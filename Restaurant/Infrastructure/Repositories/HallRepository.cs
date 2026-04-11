using Microsoft.EntityFrameworkCore;
using Restaurant.Application.Interfaces;
using Restaurant.Domain.Entities;

namespace Restaurant.Infrastructure.Repositories
{
    public class HallRepository : IHallRepository
    {
        private readonly AppDbContext _context;

        public HallRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Hall?> GetByIdAsync(Guid id)
            => await _context.Halls.FindAsync(id);

        public async Task<List<Hall>> GetAllAsync()
            => await _context.Halls.ToListAsync();

        public IQueryable<Hall> Query()
            => _context.Halls.AsQueryable();

        public async Task AddAsync(Hall hall)
            => await _context.Halls.AddAsync(hall);

        public void Update(Hall hall)
            => _context.Halls.Update(hall);

        public void Remove(Hall hall)
            => _context.Halls.Remove(hall);

        public async Task SaveChangesAsync(CancellationToken ct = default)
            => await _context.SaveChangesAsync(ct);
    }
}
