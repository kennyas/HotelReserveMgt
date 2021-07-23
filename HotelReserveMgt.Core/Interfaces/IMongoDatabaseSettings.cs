using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReserveMgt.Core.Domain
{
    public interface IMongoDatabaseSettings
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        string CollectionName { get; set; }
    }
}
