using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReserveMgt.Core.Interfaces
{
    public interface IDateTimeService
    {
        DateTime NowUtc { get; }
    }
}
