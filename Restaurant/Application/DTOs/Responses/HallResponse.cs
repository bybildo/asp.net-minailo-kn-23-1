using Restaurant.Domain.Entities;

namespace Application.DTOs.Responses
{
    public record HallResponse
    {
        public Guid Id;
        public string Name;
        public int Width;
        public int Length;

        public HallResponse(Hall hall)
        {
            Id = hall.Id;
            Name = hall.Name;
            Width = hall.Width;
            Length = hall.Length;
        }
    }
}
