using Restaurant.Application.DTOs.Requests;
using Restaurant.Domain.Entities;

namespace Restaurant.Application.Interfaces
{
    public interface ITableService
    {
        Task AddTable(AddTableRequest request);
        Task DeleteTable(Guid id);
        Task<List<Table>> GetAllTables();
        Task<Table?> GetTableById(Guid id);
        Task<List<Table>> GetTablesByHallId(Guid hallId);
    }
}
