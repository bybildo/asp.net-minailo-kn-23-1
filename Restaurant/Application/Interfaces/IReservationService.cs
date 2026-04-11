using Application.DTOs.Responses;
using Restaurant.Application.DTOs.Requests;
using Restaurant.Domain.Entities;

namespace Restaurant.Application.Interfaces
{
    public interface IReservationService
    {
        Task AddReservation(AddReservationRequest request);
        Task UpdateReservation(UpdateReservationRequest request);
        Task DeleteReservation(Guid reservationId);
        Task<List<ReservationResponse>> GetAllReservations();
        Task<ReservationResponse?> GetReservationById(Guid id);
    }
}
