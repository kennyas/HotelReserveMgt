using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReserveMgt.Core.Enums
{
    public enum RoomTrigger 
    { 
        AddRoom, 
        Assigned,
        Booked,
        RoomKeyObtained,
        Transferred,
        Released,
        Unavailable,
        Clean
    };

}
