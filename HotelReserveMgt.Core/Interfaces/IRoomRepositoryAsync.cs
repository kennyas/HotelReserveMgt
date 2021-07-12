using HotelReserveMgt.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HotelReserveMgt.Core.Interfaces
{
    public interface IRoomRepositoryAsync : IGenericRepositoryAsync<Room>
    {
        Task<bool> IsUniqueRoomcodeAsync(string barcode);
    }
}
