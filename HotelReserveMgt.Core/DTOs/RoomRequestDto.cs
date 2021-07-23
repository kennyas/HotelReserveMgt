using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReserveMgt.Core.DTOs
{
    public class RoomRequestDto
    {
        public string Name { get; set; }
        public string RoomCode { get; set; }
        public string Description { get; set; }
        public decimal LodgeFee { get; set; }
    }
}
