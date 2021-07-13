﻿using HotelReserveMgt.Core.Application.Configurations;
using HotelReserveMgt.Core.Interfaces;
using HotelReserveMgt.Infrastructure.Persistence.Contexts;
using HotelReserveMgt.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReserveMgt.Infrastructure.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("ApplicationDb"));
            }
            else
            {
                // services.AddDbContext<ApplicationDbContext>(options =>
                //options.UseSqlServer(
                //    configuration.GetConnectionString("DefaultConnection"),
                //    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
                services.Configure<RoomDatabaseConfiguration>(configuration.GetSection("RoomDatabaseConfiguration"));
            }
            #region Repositories
            services.AddTransient(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));
            services.AddTransient<IRoomRepositoryAsync, RoomRepositoryAsync>();
            #endregion
        }
    }
}
