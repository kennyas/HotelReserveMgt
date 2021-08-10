using HotelReserveMgt.Core.Domain.Entities;
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
    public class WorkflowController : ControllerBase
    {
        private readonly IRoomWorkflow _workflowService;
        public WorkflowController(IRoomWorkflow workflowService)
        {
            _workflowService = workflowService;
        }
        [HttpPost("create-asset")]
        public IActionResult CreateAsset(Room request)
        {
            var origin = Request.Headers["origin"];
            _workflowService.RoomAdded();
            return Ok();
        }
    }
}
