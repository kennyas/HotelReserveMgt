using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using HotelReserveMgt.API.Extensions;
using HotelReserveMgt.API.Services;
using HotelReserveMgt.Core.Interfaces;
using HotelReserveMgt.Core.Mappings;
using HotelReserveMgt.Infrastructure.Identity;
using HotelReserveMgt.Infrastructure.Persistence;
using HotelReserveMgt.Infrastructure.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelReserveMgt.API
{
    public class Startup
    {
        public IConfiguration _config { get; }
        public Startup(IConfiguration configuration)
        {
            //Configuration = configuration;
            _config = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var mongoConnection = _config["HotelMgtDatabaseSettings:ConnectionString"];
            services.AddApplicationLayer();
            services.AddIdentityInfrastructure(_config);
            services.AddPersistenceInfrastructure(_config);
            services.AddSharedInfrastructure(_config);
            services.AddSwaggerExtension();
            //var migrationOptions = new MongoMigrationOptions
            //{
            //    MigrationStrategy = new MigrateMongoMigrationStrategy(),
            //    BackupStrategy = new CollectionMongoBackupStrategy()
            //};
            //services.AddHangfire(config =>
            //{
            //    config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170);
            //    config.UseSimpleAssemblyNameTypeSerializer();
            //    config.UseRecommendedSerializerSettings();
            //    config.UseMongoStorage(mongoConnection, "Hangfire", new MongoStorageOptions { MigrationOptions = migrationOptions });

            //});
            var mongoUrlBuilder = new MongoUrlBuilder(mongoConnection);
            var mongoClient = new MongoClient(mongoUrlBuilder.ToMongoUrl());

            // Add Hangfire services. Hangfire.AspNetCore nuget required
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseMongoStorage(mongoClient, mongoUrlBuilder.DatabaseName, new MongoStorageOptions
                {
                    MigrationOptions = new MongoMigrationOptions
                    {
                        MigrationStrategy = new MigrateMongoMigrationStrategy(),
                        BackupStrategy = new CollectionMongoBackupStrategy()
                    },
                    Prefix = "hangfire.mongo",
                    CheckConnection = false
                })
            );

            services.AddHangfireServer();
            services.AddAutoMapper(typeof(RoomProfile));
            services.AddControllers();
            services.AddApiVersioningExtension();
            services.AddHealthChecks();
            services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseHangfireDashboard("/jobs");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwaggerExtension();
            app.UseErrorHandlingMiddleware();
            app.UseHealthChecks("/health");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
