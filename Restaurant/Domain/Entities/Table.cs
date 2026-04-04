namespace Restaurant.Domain.Entities
{
    public class Table
    {
        public Guid Id { get; set; }
        public Guid HallId { get; set; }
        public Hall Hall { get; set; } = null!;
        public int Number { get; set; }
        public int Capacity { get; set; }
        public int WidthPosition { get; set; }
        public int LengthPosition { get; set; }
    }
}
