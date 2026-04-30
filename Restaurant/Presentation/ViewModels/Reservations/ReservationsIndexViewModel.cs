using Application.DTOs.Responses;

namespace Restaurant.Presentation.ViewModels.Reservations;

public class ReservationsIndexViewModel
{
    public List<ReservationResponse> Reservations { get; set; } = [];
}
