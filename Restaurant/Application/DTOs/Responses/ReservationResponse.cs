using Restaurant.Domain.Entities;

namespace Application.DTOs.Responses
{
    public record ReservationResponse
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public ICollection<Table> Tables { get; set; } = new List<Table>();
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ReservationResponse(Reservation reservation)
        {
            Id = reservation.Id;
            UserId = reservation.UserId;
            Tables = reservation.Tables;
            StartDate = reservation.StartDate;
            EndDate = reservation.EndDate;
        }
    }
}
