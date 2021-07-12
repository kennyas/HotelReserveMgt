using HotelReserveMgt.Core.DTOs.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReserveMgt.Core.Interfaces
{
    public interface IRoomWorkflow
    {
        void OnEntry();
        void OnActivate();
        void OnDeactivate();
        void OnExit();
        void Assign(RegisterRequest owner);
        void Transfer(RegisterRequest owner);
    }
}
