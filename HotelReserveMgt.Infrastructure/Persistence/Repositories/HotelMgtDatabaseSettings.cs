using HotelReserveMgt.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelReserveMgt.Infrastructure.Persistence.Repositories
{
    public class HotelMgtDatabaseSettings:IMongoDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string RoomCollectionName { get; set; }
        public string RoomReservationCollectionName { get; set; }
        public string CustomerCollectionName { get; set; }
    }
}
