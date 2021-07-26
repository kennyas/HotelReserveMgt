using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelReserveMgt.Infrastructure.Shared.Constants.Permission
{
    public static class PermissionModules
    {
        public static List<string> GeneratePermissionsForModule(string module)
        {
            return new List<string>()
            {
                $"Permissions.{module}.Create",
                $"Permissions.{module}.View",
                $"Permissions.{module}.Edit",
                $"Permissions.{module}.Delete"
            };
        }

        public static List<string> GetAllPermissionsModules()
        {
            return new List<string>()
            {
                Users,
                Roles,
                Customers,
                Rooms
            };
        }

        public const string Users = "Users";
        public const string Roles = "Roles";
        public const string Customers = "Customers";
        public const string Rooms = "Rooms";
    }
}
