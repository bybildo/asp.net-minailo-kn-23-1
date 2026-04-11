using Application.DTOs.Responses;
using Restaurant.Application.DTOs.Requests;
using Restaurant.Application.Exceptions;
using Restaurant.Application.Interfaces;
using Restaurant.Domain.Entities;

namespace Restaurant.Application.Services
{
    public class TableService : ITableService
    {
        private readonly ITableRepository _tableRepository;
        private readonly IHallRepository _hallRepository;

        public TableService(ITableRepository tableRepository, IHallRepository hallRepository)
        {
            _tableRepository = tableRepository;
            _hallRepository = hallRepository;
        }

        public async Task AddTable(AddTableRequest request)
        {
            var hall = await _hallRepository.GetByIdAsync(request.HallId);

            if (hall == null)
                throw new NotFoundException("Hall not found");

            var isNumberExists = await _tableRepository.NumberExistsInHallAsync(request.Number, request.HallId);
            var isPositionExists = await _tableRepository.PositionExistsInHallAsync(request.WidthPosition, request.LengthPosition, request.HallId);

            if (isNumberExists)
                throw new IncorrectDataEnteredException("Number already exists in this hall.");

            if (isPositionExists)
                throw new IncorrectDataEnteredException("Position already exists in this hall.");

            if (request.WidthPosition < 0 || request.WidthPosition > hall.Width
                || request.LengthPosition < 0 || request.LengthPosition > hall.Length)
                throw new IncorrectDataEnteredException("Table position is out of bounds.");

            if (request.Capacity < 0 || request.Capacity > 10)
                throw new IncorrectDataEnteredException("Capacity must be between 0 and 10.");

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

            await _tableRepository.AddAsync(table);
            await _tableRepository.SaveChangesAsync();
        }

        public async Task DeleteTable(Guid id)
        {
            var table = await _tableRepository.GetByIdAsync(id);
            if (table == null)
                throw new NotFoundException("Table not found");

            _tableRepository.Remove(table);
            await _tableRepository.SaveChangesAsync();
        }

        public async Task<List<TableResponse>> GetTablesByHallId(Guid hallId)
        {
            var tables = await _tableRepository.GetByHallIdAsync(hallId) ?? new List<Table>();
            return tables.Select(t => new TableResponse(t)).ToList();
        }

        public async Task<TableResponse?> GetTableById(Guid id)
        {
            var table = await _tableRepository.GetByIdAsync(id);
            return table == null ? null : new TableResponse(table);
        }


        public async Task<List<TableResponse>> GetAllTables()
        {
            var tables = await _tableRepository.GetAllAsync() ?? new List<Table>();
            return tables.Select(t => new TableResponse(t)).ToList();
        }
    }
}
