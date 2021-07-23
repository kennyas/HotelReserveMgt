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
    public class ClientService: IGenericRepositoryAsync<Customer>, IClientService
    {
        private readonly IMongoCollection<Customer> _context;

        public ClientService(IMongoDatabaseSettings settings)
        {
            //_settings = settings;
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _context = database.GetCollection<Customer>(settings.CollectionName);
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            return await _context.Find(c => true).ToListAsync();
        }

        public async Task<Customer> GetByIdAsync(string id)
        {
            return await _context.Find<Customer>(id).FirstOrDefaultAsync();
        }

        public async Task<Customer> CreateAsync(Customer obj)
        {
            await _context.InsertOneAsync(obj);
            return obj;
        }

        public async Task UpdateAsync(string id, Customer entity)
        {
            await _context.ReplaceOneAsync(id, entity);
        }

        public async Task DeleteAsync(string id)
        {
            await _context.DeleteOneAsync(id);
        }
    }
}
