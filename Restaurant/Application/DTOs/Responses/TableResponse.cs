using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Responses
{
    public record TableResponse
    {
        public Guid Id { get; set; }
        public Guid HallId { get; set; }
        public Hall Hall { get; set; }
        public int Number { get; set; }
        public int Capacity { get; set; }
        public int WidthPosition { get; set; }
        public int LengthPosition { get; set; }

        public TableResponse(Table table)
        {
            Id = table.Id;
            HallId = table.HallId;
            Hall = table.Hall;
            Number = table.Number;
            Capacity = table.Capacity;
            WidthPosition = table.WidthPosition;
            LengthPosition = table.LengthPosition;
        }
    }
}
