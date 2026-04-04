using Application.DTOs.Responses;
using Microsoft.EntityFrameworkCore;
using Restaurant.Application.DTOs.Requests;
using Restaurant.Application.Exceptions;
using Restaurant.Application.Interfaces;
using Restaurant.Domain.Entities;

namespace Restaurant.Application.Services
{
    public class HallService : IHallService
    {
        private readonly IHallRepository _hallRepository;
        private readonly ITableRepository _tableRepository;

        public HallService(IHallRepository hallRepository, ITableRepository tableRepository)
        {
            _hallRepository = hallRepository;
            _tableRepository = tableRepository;
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

            await _hallRepository.AddAsync(newHall);
            await _hallRepository.SaveChangesAsync();
        }

        public async Task UpdateHall(UpdateHallRequest request)
        {
            var hall = await _hallRepository.GetByIdAsync(request.Id);

            if (hall == null)
                throw new NotFoundException("Hall not found");

            if (request.Name != null)
                hall.Name = request.Name;

            if (request.Width != null)
            {
                var maxTableWidth = await _tableRepository.GetMaxWidthPositionInHallAsync(request.Id);
                if ((int)request.Width >= maxTableWidth)
                    hall.Width = (int)request.Width;
                else
                    throw new IncorrectDataEnteredException("You have a table that is further than your width.");
            }

            if (request.Length != null)
            {
                var maxTableLength = await _tableRepository.GetMaxLengthPositionInHallAsync(request.Id);
                if ((int)request.Length >= maxTableLength)
                    hall.Length = (int)request.Length;
                else
                    throw new IncorrectDataEnteredException("You have a table that is further than your length.");
            }

            _hallRepository.Update(hall);
            await _hallRepository.SaveChangesAsync();
        }

        public async Task DeleteHall(Guid id)
        {
            var hall = await _hallRepository.GetByIdAsync(id);
            if (hall == null)
                throw new NotFoundException("Hall not found");

            _hallRepository.Remove(hall);
            await _hallRepository.SaveChangesAsync();
        }

        public async Task<List<HallResponse>> GetAllHalls()
        {
            var halls = await _hallRepository.GetAllAsync() ?? new List<Hall>();
            return halls.Select(h => new HallResponse(h)).ToList();
        }

        public async Task<HallResponse?> GetHallByRequest(SearchHallRequest request)
        {
            IQueryable<Hall> query = _hallRepository.Query();

            if (request.Id != null)
                query = query.Where(h => h.Id == request.Id.Value);

            if (!string.IsNullOrWhiteSpace(request.Name))
                query = query.Where(h => h.Name == request.Name);

            var hall = await query.FirstOrDefaultAsync();
            return hall == null ? null : new HallResponse(hall);
        }
    }
}
