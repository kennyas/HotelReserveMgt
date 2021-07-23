using HotelReserveMgt.Core.Domain.Entities;
using HotelReserveMgt.Core.DTOs;
using HotelReserveMgt.Core.Wrappers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HotelReserveMgt.Core.Interfaces
{
    public interface IRoomService: IGenericRepositoryAsync<Room>
    {
        Task<List<Room>> GetAllFreeAsync();
        Task<List<Room>> GetAllOccupiedAsync();
        Task<List<Room>> GetAllCheckedInAsync();
        Task<List<Room>> GetAllCheckedOutAsync();
        Task<List<Room>> GetAllRevenueAsync();
        //Task<Response<string>> SetUpRoomAsync(RoomRequestDto request);
    }
}
