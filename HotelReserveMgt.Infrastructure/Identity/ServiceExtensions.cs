﻿using HotelReserveMgt.Core.Application.Configurations;
using HotelReserveMgt.Core.Interfaces;
using HotelReserveMgt.Core.Wrappers;
using HotelReserveMgt.Infrastructure.Contexts;
using HotelReserveMgt.Infrastructure.Models;
using HotelReserveMgt.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using HotelReserveMgt.Infrastructure.Persistence.Repositories;
using HotelReserveMgt.Core.Domain;
using Microsoft.Extensions.Options;
using AspNetCore.Identity.Mongo;
using AspNetCore.Identity.Mongo.Model;

namespace HotelReserveMgt.Infrastructure.Identity
{
    public static class ServiceExtensions
    {
        public static void AddIdentityInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var mongoConnection = configuration["HotelMgtDatabaseSettings:ConnectionString"]; //configuration.GetConnectionString("HotelMgtDatabaseSettings:ConnectionString");
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<IdentityContext>(options =>
                    options.UseInMemoryDatabase("IdentityDb"));
            }
            else
            {
                //services.AddDbContext<IdentityContext>(options =>
                //options.UseSqlServer(
                //    configuration.GetConnectionString("IdentityConnection"),
                //    b => b.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName)));
                services.AddDbContext<IdentityContext>(options =>
                   options.UseInMemoryDatabase("IdentityDb"));
                services.Configure<HotelMgtDatabaseSettings>(configuration.GetSection(nameof(HotelMgtDatabaseSettings)));
                services.AddSingleton<IMongoDatabaseSettings>(x => x.GetRequiredService<IOptions<HotelMgtDatabaseSettings>>().Value);
            }
            services.AddIdentityMongoDbProvider<AppUser, MongoRole>(identity =>
            {
                identity.Password.RequiredLength = 8;
            },
            mongo =>
            {
                mongo.ConnectionString = mongoConnection;
                //mongo.UsersCollection = "AppUser";
                //mongo.RolesCollection = "MongoRole";
            });
            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<IdentityContext>().AddDefaultTokenProviders();
            #region Services
            services.AddTransient<IAccountService, AccountService>();
            #endregion
            services.Configure<JWTConfiguration>(configuration.GetSection("JWTSettings"));
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = configuration["JWTSettings:Issuer"],
                        ValidAudience = configuration["JWTSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSettings:Key"]))
                    };
                    o.Events = new JwtBearerEvents()
                    {
                        OnAuthenticationFailed = c =>
                        {
                            c.NoResult();
                            c.Response.StatusCode = 500;
                            c.Response.ContentType = "text/plain";
                            return c.Response.WriteAsync(c.Exception.ToString());
                        },
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";
                            var result = JsonConvert.SerializeObject(new Response<string>("You are not Authorized"));
                            return context.Response.WriteAsync(result);
                        },
                        OnForbidden = context =>
                        {
                            context.Response.StatusCode = 403;
                            context.Response.ContentType = "application/json";
                            var result = JsonConvert.SerializeObject(new Response<string>("You are not authorized to access this resource"));
                            return context.Response.WriteAsync(result);
                        },
                    };
                });
        }
    }
}
