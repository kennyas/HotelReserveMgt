using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using HotelReserveMgt.Core.Application.Configurations;
using HotelReserveMgt.Core.Domain;
using HotelReserveMgt.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelReserveMgt.Hangfire
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }



        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //var mongoConnection = Configuration.GetConnectionString("HotelMgtDatabaseSettings");
            var mongoConnection = Configuration["HotelMgtDatabaseSettings:ConnectionString"];
            //services.AddHangfire(x => x.UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection")));
            

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
            //services.Configure<HotelMgtDatabaseSettings>(Configuration.GetSection(nameof(HotelMgtDatabaseSettings)));
            //services.AddSingleton<IMongoDatabaseSettings>(x => x.GetRequiredService<IOptions<HotelMgtDatabaseSettings>>().Value);

            //    var migrationOptions = new MongoMigrationOptions
            //    {
            //        MigrationStrategy = new MigrateMongoMigrationStrategy(),
            //        BackupStrategy = new CollectionMongoBackupStrategy()
            //    };

            //services.AddHangfire(config =>
            //{
            //    config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170);
            //    config.UseSimpleAssemblyNameTypeSerializer();
            //    config.UseRecommendedSerializerSettings();
            //    config.UseMongoStorage(mongoConnection, "Hangfire", new MongoStorageOptions { MigrationOptions = migrationOptions });

            //});
            services.AddHangfireServer();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HotelReserveMgt.Hangfire", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HotelReserveMgt.Hangfire v1"));
            }
            app.UseHangfireDashboard("/hotelreservemgtdashboard");
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
