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
        private readonly IReservationService _reservationService;
        public RoomService( IReservationService reservationService, IMongoDatabaseSettings settings)
        {
            _reservationService = reservationService;
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _context = database.GetCollection<Room>(settings.CollectionName);
        }

        public async Task<List<Room>> GetAllAsync()
        {
            return await _context.Find(c => true).ToListAsync();
        }
        public async Task<List<Room>> GetAllFreeAsync()
        {
            return await _context.Find(c =>c.IsActive == false).ToListAsync();
        }
        public async Task<List<Room>> GetAllOccupiedAsync()
        {
            return await _context.Find(c => c.IsActive == true).ToListAsync();
        }
        public async Task<List<Room>> GetAllCheckedInAsync()
        {
            var bookedList = new List<Room>();
            var queryRes = await _context.Find(c => c.IsActive == true).ToListAsync();
            foreach(var item in queryRes)
            {
                var booked = await _reservationService.GetByIdAsync(item.Id.ToString());
                if(booked.ReservationStatus == "Occupied")
                {
                    bookedList.Add(item);
                }
            }
            return bookedList; //await _context.Find(c => c.IsActive == true).ToListAsync();
        }

        public async Task<List<Room>> GetAllCheckedOutAsync()
        {
            var bookedList = new List<Room>();
            var queryRes = await _context.Find(c => c.IsActive == true).ToListAsync();
            foreach (var item in queryRes)
            {
                var booked = await _reservationService.GetByIdAsync(item.Id.ToString());
                if (booked.ReservationStatus == "Released")
                {
                    bookedList.Add(item);
                }
            }
            return bookedList; 
        }

        public async Task<List<Room>> GetAllRevenueAsync()
        {
            var bookedList = new List<Room>();
            var queryRes = await _context.Find(c => c.IsActive == true).ToListAsync();
            foreach (var item in queryRes)
            {
                var booked = await _reservationService.GetByIdAsync(item.Id.ToString());
                if (booked.ReservationStatus == "Booked")
                {
                    bookedList.Add(item);
                }
            }
            return bookedList;
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

       

    }
}
