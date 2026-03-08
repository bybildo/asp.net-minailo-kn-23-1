using Default.Application.DTOs.Requests;
using Default.Application.Exceptions;
using Default.Application.Exeptions;
using Default.Infrastructure;
using Default.Models;
using Microsoft.EntityFrameworkCore;

namespace Default.Application.Services
{
    public class HallService
    {
        private readonly AppDbContext _context;

        public HallService(AppDbContext context)
        {
            _context = context;
        }

        public async Task UpdateHall(UpdateHallRequest request)
        {
            var hall = await _context.Halls.FindAsync(request.Id);

            if (hall == null)
                throw new NotFoundException("Hall not found");

            if (request.Name != null)
                hall.Name = request.Name;

            if (request.Width != null)
            {
                int maxTableWidth = await _context.Tables.Where(t => t.HallId == request.Id).MaxAsync(t => t.WidthPosition);
                if ((int)request.Width > maxTableWidth)
                    hall.Width = (int)request.Width;
                else
                    throw new IncorrectDataEnteredException("You have a table that is further than your width.");
            }

            if (request.Length != null)
            {
                int maxTableLength = await _context.Tables.Where(t => t.HallId == request.Id).MaxAsync(t => t.LengthPosition);
                if ((int)request.Length > maxTableLength)
                    hall.Length = (int)request.Length;
                else
                    throw new IncorrectDataEnteredException("You have a table that is further than your length.");
            }

            _context.Halls.Update(hall);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteHall(Guid id)
        {
            var hall = await _context.Halls.FindAsync(id);
            if (hall != null)
            {
                _context.Halls.Remove(hall);
                await _context.SaveChangesAsync();
            }
            else throw new NotFoundException("Hall not found");
        }

        public async Task AddHall(AddHallRequest request)
        {
            var newHall = new Hall
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Width = request.Width,
                Length = request.Length
            };

            await _context.Halls.AddAsync(newHall);
            await _context.SaveChangesAsync();
        }

        public async Task<Hall?> GetHallByRequest(SearchHallRequest request)
        {
            IQueryable<Hall> query = _context.Halls;

            if (request.Id != null)
            {
                query = query.Where(h => h.Id == request.Id.Value);
            }

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                query = query.Where(h => h.Name == request.Name);
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<Hall>> GetAllHalls()
        {
            return await _context.Halls.ToListAsync();
        }
    }
}
