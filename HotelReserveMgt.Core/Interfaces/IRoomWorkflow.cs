using HotelReserveMgt.Core.DTOs.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReserveMgt.Core.Interfaces
{
    public interface IRoomWorkflow
    {
        void RoomCleaned();
        void RoomBooked();
        void RoomReleased();
        void RoomAssigned();
        void RoomAdded();
        void Unavailable();
    }
}
