using Restaurant.Domain.Entities;

namespace Application.DTOs.Responses
{
    public record HallResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Width { get; set; }
        public int Length { get; set; }

        public HallResponse(Hall hall)
        {
            Id = hall.Id;
            Name = hall.Name;
            Width = hall.Width;
            Length = hall.Length;
        }
    }
}
