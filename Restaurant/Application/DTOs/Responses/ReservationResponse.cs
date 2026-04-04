using Restaurant.Domain.Entities;
using System.Diagnostics.Tracing;

namespace Application.DTOs.Responses
{
    public record ReservationResponse
    {
        public Guid Id { get; set; }
        public string HallName { get; set; }
        public ICollection<Guid> TablesIds { get; set; } = new List<Guid>();   
        public ICollection<int> TablesNumbers { get; set; } = new List<int>();
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }

        public ReservationResponse(Reservation reservation)
        {
            Id = reservation.Id;
            HallName = reservation.Tables?.First()?.Hall?.Name;
            TablesIds = reservation.Tables.Select(t => t.Id).ToList();
            TablesNumbers = reservation.Tables.Select(t => t.Number).ToList();
            StartDate = reservation.StartDate;
            EndDate = reservation.EndDate;

            if (DateTime.Now < StartDate)
            {
                Status = "Очікує";
            }
            else if (DateTime.Now >= StartDate && DateTime.Now <= EndDate)
            {
                Status = "Активне";
            }
            else
            {
                Status = "Завершено";
            }
        }
    }
}
