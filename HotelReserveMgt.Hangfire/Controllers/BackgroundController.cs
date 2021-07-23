using Hangfire;
using HotelReserveMgt.Core.Domain.Entities;
using HotelReserveMgt.Core.DTOs.Email;
using HotelReserveMgt.Core.Exceptions;
using HotelReserveMgt.Core.Interfaces;
using HotelReserveMgt.Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelReserveMgt.Hangfire.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BackgroundController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRoomService _roomService;
        public BackgroundController(IEmailService emailService, IRoomService roomService, UserManager<ApplicationUser> userManager)
        {
            _emailService = emailService;
            _roomService = roomService;
            _userManager = userManager;

        }
        [HttpPost]
        [Route("registrationEmail")]
        public IActionResult RegistrationEmail(string userName)
        {
            var origin = Request.Headers["origin"];
            var jobId = BackgroundJob.Enqueue(() => SendRegistrationMail(userName, origin));
            return Ok($"Job Id {jobId} Completed. Welcome Mail Sent!");
        }
        [HttpPost]
        [Route("roomConfirmationEmail")]
        public IActionResult RoomConfirmationEmail(string roomId)
        {
            var origin = Request.Headers["origin"];
            var jobId = BackgroundJob.Enqueue(() => SendRoomConfirmationNotification(origin));
            return Ok($"Job Id {jobId} Completed. Welcome Mail Sent!");
        }
        private async Task<string> SendVerificationEmail(ApplicationUser user, string origin)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "api/account/confirm-email/";
            var _enpointUri = new Uri(string.Concat($"{origin}/", route));
            var verificationUri = QueryHelpers.AddQueryString(_enpointUri.ToString(), "userId", user.Id);
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "code", code);
            //Email Service Call Here
            return verificationUri;
        }
        public async Task SendRegistrationMail(string userName, string origin)
        {
            //Logic to Mail the user
            var user = await _userManager.FindByNameAsync(userName);
            var verificationUri = await SendVerificationEmail(user, origin);
            if (user != null)
            {
                throw new ApiException($"Username '{userName}' does not exist.");
            }
            await _emailService.SendAsync(new EmailRequest() { From = "kehindeasishana@gmail.com", To = user.Email, Body = $"Please confirm your account by visiting this URL {verificationUri}", Subject = "Confirm Registration" });
        }

        public async Task SendRoomConfirmationNotification(string origin)
        {
            var roomList = await _roomService.GetAllAsync();
            var room = roomList.FirstOrDefault();
            var verificationUri = await SendRoomVerificationEmail(room, origin);
            if (room == null)
            {
                throw new ApiException($"Room with Id '{room.Id}' does not exist.");
            }
            await _emailService.SendAsync(new EmailRequest() { From = "kehindeasishana@gmail.com", To = room.Client.Email, Body = $"Please confirm your account by visiting this URL {verificationUri}", Subject = "Confirm Registration" });
        }

        private async Task<string> SendRoomVerificationEmail(Room room, string origin)
        {
            var code = ""; //await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "api/account/confirm-email/";
            var _enpointUri = new Uri(string.Concat($"{origin}/", route));
            var verificationUri = QueryHelpers.AddQueryString(_enpointUri.ToString(), "userId", room.Id.ToString());
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "code", code);
            return verificationUri;
        }
    }
}
