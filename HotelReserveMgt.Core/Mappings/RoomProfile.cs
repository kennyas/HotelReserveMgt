using AutoMapper;
using HotelReserveMgt.Core.Domain.Entities;
using HotelReserveMgt.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReserveMgt.Core.Mappings
{
    public class RoomProfile : Profile
    {
        public RoomProfile()
        {
            CreateMap<RoomRequestDto, Room>().ReverseMap();
        }
    }
}
