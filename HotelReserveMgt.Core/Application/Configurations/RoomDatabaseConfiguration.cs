﻿using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReserveMgt.Core.Application.Configurations
{
    public class RoomDatabaseConfiguration
    {
        public string RoomCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
