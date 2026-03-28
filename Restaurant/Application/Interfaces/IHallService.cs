using Application.DTOs.Responses;
using Restaurant.Application.DTOs.Requests;
using Restaurant.Domain.Entities;

namespace Restaurant.Application.Interfaces
{
    public interface IHallService
    {
        Task AddHall(AddHallRequest request);
        Task UpdateHall(UpdateHallRequest request);
        Task DeleteHall(Guid id);
        Task<List<HallResponse>> GetAllHalls();
        Task<HallResponse> GetHallByRequest(SearchHallRequest request);
    }
}
