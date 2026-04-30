using Application.DTOs.Responses;

namespace Restaurant.Presentation.ViewModels.Tables;

public class TablesIndexViewModel
{
    public Guid? UserId { get; set; }
    public List<HallResponse> Halls { get; set; } = [];
    public List<TableResponse> Tables { get; set; } = [];
    public List<ReservationResponse> Reservations { get; set; } = [];
}
