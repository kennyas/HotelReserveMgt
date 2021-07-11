using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReserveMgt.Core.Interfaces
{
    public interface IAuthenticatedUserService
    {
        string UserId { get; }
    }
}
