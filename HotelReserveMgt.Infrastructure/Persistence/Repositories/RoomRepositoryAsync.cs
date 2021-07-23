using HotelReserveMgt.Core.Domain.Entities;
using HotelReserveMgt.Core.Interfaces;
using HotelReserveMgt.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HotelReserveMgt.Infrastructure.Persistence.Repositories
{
    //public class RoomRepositoryAsync : GenericRepositoryAsync<Room>, IRoomRepositoryAsync
    //{
    //    private readonly DbSet<Room> _rooms;

    //    public RoomRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
    //    {
    //        _rooms = dbContext.Set<Room>();
    //    }

    //    public Task<bool> IsUniqueRoomcodeAsync(string roomcode)
    //    {
    //        return _rooms
    //            .AllAsync(p => p.RoomCode != roomcode);
    //    }
    //}
}
