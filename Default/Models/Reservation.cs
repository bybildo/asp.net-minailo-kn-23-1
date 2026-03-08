namespace Default.Models
{
    public class Reservation
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public ICollection<Table> Tables { get; set; } = new List<Table>();
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
