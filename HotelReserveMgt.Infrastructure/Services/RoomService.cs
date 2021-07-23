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
    //public class RoomService : GenericRepositoryAsync<Room>, IRoomService
    //{
    //    private readonly IMongoCollection<Room> _context;
    //    private readonly IMongoDatabaseSettings _settings;

    //    //public RoomService(ApplicationDbContext dbContext) : base(dbContext)
    //    //{
    //    //    //_rooms = dbContext.Set<Room>();
    //    //}
    //    //public RoomService(IMongoDatabaseSettings settings)
    //    //{
    //    //    //_settings = settings;
    //    //    var client = new MongoClient(settings.ConnectionString);
    //    //    var database = client.GetDatabase(settings.DatabaseName);
    //    //    _context = database.GetCollection<Room>(settings.CollectionName);
    //    //}

       
    //}
}
