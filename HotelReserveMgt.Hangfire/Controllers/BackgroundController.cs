using Hangfire;
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
        public BackgroundController(IEmailService emailService, UserManager<ApplicationUser> userManager)
        {
            _emailService = emailService;
            _userManager = userManager;

        }
        [HttpPost]
        [Route("sendEmail")]
        public IActionResult SendEmail(string userName)
        {
            var origin = Request.Headers["origin"];
            var jobId = BackgroundJob.Enqueue(() => SendRegistrationMail(userName), origin);
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
        
        [HttpPost]
        [Route("unsubscribe")]
        public IActionResult Unsubscribe(string userName)
        {
            var jobId = BackgroundJob.Enqueue(() => UnsubscribeUser(userName));
            BackgroundJob.ContinueJobWith(jobId, () => Console.WriteLine($"Sent Confirmation Mail to {userName}"));
            return Ok($"Unsubscribed");
        }

        public void UnsubscribeUser(string userName)
        {
            //Logic to Unsubscribe the user
            Console.WriteLine($"Unsubscribed {userName}");
        }
    }
}
