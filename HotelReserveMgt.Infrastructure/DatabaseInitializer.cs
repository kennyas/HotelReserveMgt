using HotelReserveMgt.Infrastructure.Contexts;
using HotelReserveMgt.Infrastructure.Helpers;
using HotelReserveMgt.Infrastructure.Models;
using HotelReserveMgt.Infrastructure.Shared.Constants;
using HotelReserveMgt.Infrastructure.Shared.Constants.Permission;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelReserveMgt.Infrastructure
{
    public class DatabaseInitializer
    {
        private readonly ILogger<DatabaseInitializer> _logger;
        private readonly IdentityContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DatabaseInitializer(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IdentityContext db, ILogger<DatabaseInitializer> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
            _logger = logger;
        }
        public void Initialize()
        {
            AddAdministrator();
            _db.SaveChanges();
        }

        private void AddAdministrator()
        {
            Task.Run(async () =>
            {
                //Check if Role Exists
                var adminRole = new IdentityRole(Role.AdministratorRole);
                var adminRoleInDb = await _roleManager.FindByNameAsync(Role.AdministratorRole);
                if (adminRoleInDb == null)
                {
                    await _roleManager.CreateAsync(adminRole);
                    _logger.LogInformation("Seeded Administrator Role.");
                }
                //Check if User Exists
                var superUser = new ApplicationUser
                {
                    FirstName = "Kehinde",
                    LastName = "Asishana",
                    Email = "kehindeasishana@gmail.com",
                    UserName = "kennyrugged",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    DateCreated = DateTime.Now,
                    IsActive = true
                };
                var superUserInDb = await _userManager.FindByEmailAsync(superUser.Email);
                if (superUserInDb == null)
                {
                    await _userManager.CreateAsync(superUser, Role.DefaultPassword);
                    var result = await _userManager.AddToRoleAsync(superUser, Role.AdministratorRole);
                    if (result.Succeeded)
                    {
                        await _roleManager.GeneratePermissionClaimByModule(adminRole, PermissionModules.Users);
                        await _roleManager.GeneratePermissionClaimByModule(adminRole, PermissionModules.Roles);
                        await _roleManager.GeneratePermissionClaimByModule(adminRole, PermissionModules.Customers);
                        await _roleManager.GeneratePermissionClaimByModule(adminRole, PermissionModules.Rooms);
                    }
                    _logger.LogInformation("Seeded User with Administrator Role.");
                }
            }).GetAwaiter().GetResult();
        }
    }
}
