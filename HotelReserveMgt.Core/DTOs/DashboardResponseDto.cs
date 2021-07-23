using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReserveMgt.Core.DTOs
{
    public class DashboardResponseDto
    {
        public long TotalRoomCount { get; set; }
        public long TotalOccupiedRooms { get; set; }
        public long TotalFreeRooms { get; set; }
        public decimal TotalRevenue { get; set; }
        public long TotalCheckIns { get; set; }
        public long TotalCheckOuts { get; set; }
    }
}
