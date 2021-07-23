using AutoMapper;
using HotelReserveMgt.Core.Domain.Entities;
using HotelReserveMgt.Core.DTOs;
using HotelReserveMgt.Core.DTOs.Account;
using HotelReserveMgt.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelReserveMgt.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IRoomService _roomService;
        private readonly IClientService _clientService;
        private readonly IMapper _mapper;
        public AdminController(IRoomService roomService, IClientService clientService, IMapper mapper)
        {
            _roomService = roomService;
            _clientService = clientService;
            _mapper = mapper;
        }
        [HttpPost("setuproom")]
        public async Task<IActionResult> SetupRoomAsync(RoomRequestDto request)
        {
            var room = _mapper.Map<Room>(request);
            //var res = await _roomService.CreateAsync(room);
            //if (res != null) res.RoomStatus = true;
            return Ok(await _roomService.CreateAsync(room));
        }
        [HttpGet]
        public async Task<IActionResult> DashboardData()
        {
            
            return Ok(await _clientService.DashboardRecord());
        }
    }
}
