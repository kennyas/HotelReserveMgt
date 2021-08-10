﻿using HotelReserveMgt.Core.Domain;
using HotelReserveMgt.Core.Domain.Entities;
using HotelReserveMgt.Core.DTOs;
using HotelReserveMgt.Core.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelReserveMgt.Infrastructure.Services
{
    public class ClientService : IGenericRepositoryAsync<Customer>, IClientService
    {
        private readonly IMongoCollection<Customer> _context;
        private readonly IRoomService _roomService;
        private readonly IRoomWorkflow _roomWorkflow;
        public ClientService(IRoomService roomService, IRoomWorkflow roomWorkflow, IMongoCollection<Customer> context)
        {
            _roomService = roomService;
            _roomWorkflow = roomWorkflow;
            _context = context;
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
            _roomWorkflow.RoomAdded();
            return obj;
        }

        public async Task UpdateAsync(string id, Customer entity)
        {
            await _context.ReplaceOneAsync(id, entity);
        }

        public async Task DeleteAsync(string id)
        {
            await _context.DeleteOneAsync(id);
            _roomWorkflow.Unavailable();
        }

        public async Task<DashboardResponseDto> DashboardRecord()
        {
            var result = new DashboardResponseDto();
            var roomList = await _roomService.GetAllAsync();
            var freeRoomList = await _roomService.GetAllFreeAsync();
            var occupedRoomList = await _roomService.GetAllOccupiedAsync();
            var checkinList = await _roomService.GetAllOccupiedAsync();
            var checkOutList = await _roomService.GetAllCheckedOutAsync();
            var revenueList = await _roomService.GetAllRevenueAsync();
            var sumRevenue = revenueList.Sum(x => x.LodgeFee);
            result.TotalRoomCount = roomList.Count();
            result.TotalOccupiedRooms = occupedRoomList.Count();
            result.TotalFreeRooms = freeRoomList.Count();
            result.TotalCheckIns = checkinList.Count();
            result.TotalCheckOuts = checkOutList.Count();
            result.TotalRevenue = sumRevenue;
            return result;

        }
    }
}
