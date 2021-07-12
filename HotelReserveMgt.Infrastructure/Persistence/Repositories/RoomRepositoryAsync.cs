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
    public class RoomRepositoryAsync : GenericRepositoryAsync<Room>, IRoomRepositoryAsync
    {
        private readonly DbSet<Room> _products;

        public RoomRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _products = dbContext.Set<Room>();
        }

        public Task<bool> IsUniqueRoomcodeAsync(string roomcode)
        {
            return _products
                .AllAsync(p => p.RoomCode != roomcode);
        }
    }
}
