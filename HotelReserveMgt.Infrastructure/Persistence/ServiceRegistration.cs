using HotelReserveMgt.Core.Application.Configurations;
using HotelReserveMgt.Core.Domain;
using HotelReserveMgt.Core.Domain.Entities;
using HotelReserveMgt.Core.Interfaces;
using HotelReserveMgt.Infrastructure.Persistence.Contexts;
using HotelReserveMgt.Infrastructure.Persistence.Repositories;
using HotelReserveMgt.Infrastructure.Services;
using HotelReserveMgt.Infrastructure.WorkFlows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
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
                // services.Configure<RoomDatabaseConfiguration>(configuration.GetSection("RoomDatabaseConfiguration"));
                //services.Configure<MongoDatabaseSettings>(configuration.GetSection(nameof(MongoDatabaseSettings)));
                //services.AddSingleton<IMongoDatabaseSettings>(x => x.GetRequiredService<IOptions<MongoDatabaseSettings>>().Value);
                ConfigureMongoDb(services);
            }

            void ConfigureMongoDb(IServiceCollection services)
            {
                var settings = GetMongoDbSettings();
                services.AddSingleton(_ => CreateMongoDatabase(settings));

                AddMongoDbService<RoomService, Room>(settings.RoomCollectionName);
                AddMongoDbService<ClientService, Customer>(settings.CustomerCollectionName);
                AddMongoDbService<ReservationService, RoomReservation>(settings.RoomReservationCollectionName);

                void AddMongoDbService<TService, TModel>(string collectionName)
                {
                    services.AddSingleton(sp => sp.GetRequiredService<IMongoDatabase>().GetCollection<TModel>(collectionName));
                    services.AddSingleton(typeof(TService));
                }
            }

            HotelMgtDatabaseSettings GetMongoDbSettings() =>
    configuration.GetSection(nameof(HotelMgtDatabaseSettings)).Get<HotelMgtDatabaseSettings>();

            IMongoDatabase CreateMongoDatabase(HotelMgtDatabaseSettings settings)
            {
                var client = new MongoClient(settings.ConnectionString);
                return client.GetDatabase(settings.DatabaseName);
            }

            #region Repositories
            services.AddTransient(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));
            services.AddTransient<IClientService, ClientService>();
            services.AddTransient<IRoomService, RoomService>();
            services.AddTransient<IReservationService, ReservationService>();
            services.AddScoped<IClientWorkFlow, ClientWorkFlow>();
            services.AddScoped<IRoomWorkflow, RoomWorkflow>();
            #endregion
        }

       


    }
}
