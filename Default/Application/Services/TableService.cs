using Default.Application.DTOs.Requests;
using Default.Application.Exceptions;
using Default.Application.Exeptions;
using Default.Infrastructure;
using Default.Models;
using Microsoft.EntityFrameworkCore;

namespace Default.Application.Services
{
    public class TableService
    {
        private readonly AppDbContext _context;

        public TableService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddTable(AddTableRequest request)
        {
            var hall = await _context.Halls.FindAsync(request.HallId);

            if (hall == null)
            {
                throw new NotFoundException("Hall not found");
            }

            var isNumberExists = await _context.Tables.AnyAsync(t => t.Number == request.Number && t.HallId == request.HallId);

            if (isNumberExists)
            {
                throw new IncorrectDataEnteredException("Number already exists in this hall.");
            }

            if (request.WidthPosition < 0 || request.WidthPosition > hall.Width || request.LengthPosition < 0 || request.LengthPosition > hall.Length)
            {
                throw new IncorrectDataEnteredException("Table position is out of bounds.");
            }

            if (request.Capacity < 0 || request.Capacity > 10)
            {
                throw new IncorrectDataEnteredException("Capacity must be between 0 and 10");
            }

            var table = new Table
            {
                Id = Guid.NewGuid(),
                Number = request.Number,
                WidthPosition = request.WidthPosition,
                LengthPosition = request.LengthPosition,
                Capacity = request.Capacity,
                HallId = request.HallId,
                Hall = hall
            };

            await _context.Tables.AddAsync(table);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTable(Guid id)
        {
            var table = await _context.Tables.FindAsync(id);
            if (table != null)
            {
                _context.Tables.Remove(table);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new NotFoundException("Table not found");
            }
        }

        public async Task<List<Table>> GetTablesByHallId(Guid hallId) => await _context.Tables.Where(t => t.HallId == hallId).ToListAsync();

        public async Task<Table?> GetTableById(Guid id) => await _context.Tables.FindAsync(id);

        public async Task<List<Table>> GetAllTables() => await _context.Tables.ToListAsync();
    }
}
