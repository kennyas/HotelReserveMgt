using HotelReserveMgt.Core.Application.Configurations;
using HotelReserveMgt.Core.Domain;
using HotelReserveMgt.Core.Domain.Entities;
using HotelReserveMgt.Core.DTOs;
using HotelReserveMgt.Core.Interfaces;
using HotelReserveMgt.Core.Wrappers;
using HotelReserveMgt.Infrastructure.Persistence.Contexts;
using HotelReserveMgt.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelReserveMgt.Infrastructure.Services
{
    public class RoomService : IGenericRepositoryAsync<Room>, IRoomService
    {
        private readonly IMongoCollection<Room> _context;
        private readonly IMongoDatabaseSettings _settings;

        //public RoomService(ApplicationDbContext dbContext) : base(dbContext)
        //{
        //    //_rooms = dbContext.Set<Room>();
        //}
        public RoomService(IMongoDatabaseSettings settings)
        {
            //_settings = settings;
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _context = database.GetCollection<Room>(settings.CollectionName);
        }

        public async Task<List<Room>> GetAllAsync()
        {
            return await _context.Find(c => true).ToListAsync();
        }

        public async Task<Room> GetByIdAsync(string id)
        {
            return await _context.Find<Room>(id).FirstOrDefaultAsync();
        }

        public async Task<Room> CreateAsync(Room obj)
        {
            await _context.InsertOneAsync(obj);
            return obj;
        }

        public async Task UpdateAsync(string id, Room entity)
        {
            await _context.ReplaceOneAsync(id, entity);
        }

        public async Task DeleteAsync(string id)
        {
            await _context.DeleteOneAsync(id);
        }

        public void ReserveRoom(string userId)
        {

        }
    }
}
