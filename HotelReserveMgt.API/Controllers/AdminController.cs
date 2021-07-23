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
        private readonly IMapper _mapper;
        public AdminController(IRoomService roomService, IMapper mapper)
        {
            _roomService = roomService;
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
            var dashboardItem = new Room();
            var totalRooms = await _roomService.GetAllAsync();
            var roomCount = totalRooms.Count();
            var occupiedRooms = await _roomService.GetAllAsync();
            var occupiedCount = occupiedRooms.Count();
            var freeRoom = totalRooms.Count();
            return Ok();
        }
    }
}
