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
        public Guid Id;
        public Guid HallId;
        public Hall Hall;
        public int Number;
        public int Capacity;
        public int WidthPosition;
        public int LengthPosition;

        public TableResponse(Table table)
        {
            table.Id = table.Id;
            table.HallId = table.HallId;
            table.Hall = table.Hall;
            table.Number = table.Number;
            table.Capacity = table.Capacity;
            table.WidthPosition = table.WidthPosition;
            table.LengthPosition = table.LengthPosition;
        }
    }
}
