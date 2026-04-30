using Application.DTOs.Responses;

namespace Restaurant.Presentation.ViewModels.Admin;

public class AdminIndexViewModel
{
    public List<HallResponse> Halls { get; set; } = [];
    public List<TableResponse> Tables { get; set; } = [];
    public List<ReservationResponse> Reservations { get; set; } = [];
    public List<UserResponse> Users { get; set; } = [];
}
