using HotelReserveMgt.Core.Domain.Common;
using HotelReserveMgt.Core.DTOs.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReserveMgt.Core.Domain.Entities
{
    public class Room : AuditableBaseEntity
    {
        public string Name { get; set; }
        public string RoomCode { get; set; }
        public string Description { get; set; }
        public decimal LodgeFee { get; set; }
        public RegisterRequest Client { get; set; }
    }
}
