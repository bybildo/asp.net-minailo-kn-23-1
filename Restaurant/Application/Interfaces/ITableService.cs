using Application.DTOs.Responses;
using Restaurant.Application.DTOs.Requests;
using Restaurant.Domain.Entities;

namespace Restaurant.Application.Interfaces
{
    public interface ITableService
    {
        Task AddTable(AddTableRequest request);
        Task DeleteTable(Guid id);
        Task<List<TableResponse>> GetAllTables();
        Task<TableResponse?> GetTableById(Guid id);
        Task<List<TableResponse>> GetTablesByHallId(Guid hallId);
    }
}
