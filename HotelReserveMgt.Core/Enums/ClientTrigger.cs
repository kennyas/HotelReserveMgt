using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReserveMgt.Core.Enums
{
    public enum ClientTrigger 
    { 
        Register, 
        Occupy, 
        BookRoom, 
        CheckedOut,
        Cleaned,
        ConfirmationMail
    };
}
