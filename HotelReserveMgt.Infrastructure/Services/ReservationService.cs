using HotelReserveMgt.Core.Domain;
using HotelReserveMgt.Core.Domain.Entities;
using HotelReserveMgt.Core.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelReserveMgt.Infrastructure.Services
{
    public class ReservationService : IGenericRepositoryAsync<RoomReservation>, IReservationService
    {
        private readonly IMongoCollection<RoomReservation> _context;
        public ReservationService(IMongoDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _context = database.GetCollection<RoomReservation>(settings.CollectionName);
        }

        public async Task<List<RoomReservation>> GetAllAsync()
        {
            return await _context.Find(c => true).ToListAsync();
        }

        public async Task<RoomReservation> GetByIdAsync(string id)
        {
            return await _context.Find<RoomReservation>(id).FirstOrDefaultAsync();
        }

        public async Task<RoomReservation> CreateAsync(RoomReservation obj)
        {
            await _context.InsertOneAsync(obj);
            return obj;
        }

        public async Task UpdateAsync(string id, RoomReservation entity)
        {
            await _context.ReplaceOneAsync(id, entity);
        }

        public async Task DeleteAsync(string id)
        {
            await _context.DeleteOneAsync(id);
        }
    }
}
