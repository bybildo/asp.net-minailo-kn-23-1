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
        Task DeleteReservation(Guid reservationId, Guid requestUserId);
        Task<List<Reservation>> GetAllReservations();
        Task<ReservationResponse?> GetReservationById(Guid id);
        Task<List<ReservationResponse>> GetAllReservationsByHallId(Guid hallId);
        Task<List<ReservationResponse>> GetAllReservationsByUserId(Guid userId);
    }
}
