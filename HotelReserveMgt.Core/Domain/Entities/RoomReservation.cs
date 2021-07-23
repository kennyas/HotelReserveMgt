using HotelReserveMgt.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReserveMgt.Core.Domain.Entities
{
    public class RoomReservation:AuditableBaseEntity
    {
        public string Description { get; set; }
        public long RoomId { get; set; }
        public long CustomerId { get; set; }
        public DateTime ReservationDate { get; set; }
    }
}
