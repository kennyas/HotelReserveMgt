using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReserveMgt.Core.Interfaces
{
    public interface IClientWorkFlow
    {
        void OnClientRegistered();
        void Registered();
        void BookedRoom();
        void Occupied();
        void Checkout();
        string ToDotGraph();
    }
}
