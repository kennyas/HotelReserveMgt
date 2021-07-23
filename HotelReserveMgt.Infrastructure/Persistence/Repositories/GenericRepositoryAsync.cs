using HotelReserveMgt.Core.Application.Configurations;
using HotelReserveMgt.Core.Domain;
using HotelReserveMgt.Core.Interfaces;
using HotelReserveMgt.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelReserveMgt.Infrastructure.Persistence.Repositories
{
    public class GenericRepositoryAsync<T> : IGenericRepositoryAsync<T> where T : class
    {
        //private readonly ApplicationDbContext _dbContext;
        private readonly IMongoCollection<T> _context;
       // private readonly RoomDatabaseConfiguration _settings;
        //public GenericRepositoryAsync(ApplicationDbContext dbContext)
        //{
        //    _dbContext = dbContext;
        //}


        public GenericRepositoryAsync(IMongoDatabaseSettings settings)
        {
            //_settings = settings.Value;
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _context = database.GetCollection<T>(settings.CollectionName);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Find(c => true).ToListAsync();
        }

        public async Task<T> GetByIdAsync(string id)
        {
            return await _context.Find<T>(id).FirstOrDefaultAsync();
        }

        public async Task<T> CreateAsync(T obj)
        {
            await _context.InsertOneAsync(obj);
            return obj;
        }

        public async Task UpdateAsync(string id, T entity)
        {
            await _context.ReplaceOneAsync(id, entity);
        }

        public async Task DeleteAsync(string id)
        {
            await _context.DeleteOneAsync(id);
        }
        //public virtual async Task<T> GetByIdAsync(int id)
        //{
        //    return await _dbContext.Set<T>().FindAsync(id);
        //}

        //public async Task<IReadOnlyList<T>> GetPagedReponseAsync(int pageNumber, int pageSize)
        //{
        //    return await _dbContext
        //        .Set<T>()
        //        .Skip((pageNumber - 1) * pageSize)
        //        .Take(pageSize)
        //        .AsNoTracking()
        //        .ToListAsync();
        //}

        //public async Task<T> AddAsync(T entity)
        //{
        //    await _dbContext.Set<T>().AddAsync(entity);
        //    await _dbContext.SaveChangesAsync();
        //    return entity;
        //}

        //public async Task UpdateAsync(T entity)
        //{
        //    _dbContext.Entry(entity).State = EntityState.Modified;
        //    await _dbContext.SaveChangesAsync();
        //}

        //public async Task DeleteAsync(T entity)
        //{
        //    _dbContext.Set<T>().Remove(entity);
        //    await _dbContext.SaveChangesAsync();
        //}

        //public async Task<IReadOnlyList<T>> GetAllAsync()
        //{
        //    return await _dbContext
        //         .Set<T>()
        //         .ToListAsync();
        //}
    }
}
