using HotelReserveMgt.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReserveMgt.Core.Domain.Entities
{
    public class Room : AuditableBaseEntity
    {
        public string Name { get; set; }
        public string Barcode { get; set; }
        public string Description { get; set; }
        public decimal Rate { get; set; }
    }
}
