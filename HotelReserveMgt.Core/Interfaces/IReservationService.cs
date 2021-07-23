using HotelReserveMgt.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReserveMgt.Core.Interfaces
{
    public interface IReservationService: IGenericRepositoryAsync<RoomReservation>
    {
    }
}
