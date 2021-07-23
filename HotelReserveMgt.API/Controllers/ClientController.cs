using AutoMapper;
using HotelReserveMgt.Core.Domain.Entities;
using HotelReserveMgt.Core.DTOs;
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
    public class ClientController : ControllerBase
    {
        private readonly IRoomService _roomService;
        private readonly IClientService _clientService;
        private readonly IReservationService _reservationService;
        public ClientController(IRoomService roomService, IClientService clientService, IReservationService reservationService)
        {
            _roomService = roomService;
            _clientService = clientService;
            _reservationService = reservationService;
        }
        [HttpPost("register-customer")]
        public async Task<IActionResult> RegisterCustomerAsync([FromBody]Customer request)
        {
            //var room = _mapper.Map<Room>(request);
            return Ok(await _clientService.CreateAsync(request));
        }

        [HttpPost("bookroom")]
        public async Task<IActionResult> BookRoomAsync([FromBody] RoomReservation roomRequest)
        {
            //var room = _mapper.Map<Room>(request);
            return Ok(await _reservationService.CreateAsync(roomRequest));
        }
    }
}
