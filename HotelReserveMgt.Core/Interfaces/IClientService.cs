using HotelReserveMgt.Core.Domain.Entities;
using HotelReserveMgt.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HotelReserveMgt.Core.Interfaces
{
    public interface IClientService : IGenericRepositoryAsync<Customer>
    {
        Task<DashboardResponseDto> DashboardRecord();
    }
}
