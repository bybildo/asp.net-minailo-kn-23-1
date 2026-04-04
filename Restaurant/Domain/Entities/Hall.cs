namespace Restaurant.Domain.Entities
{
    public class Hall
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Width { get; set; }
        public int Length { get; set; }
    }
}
