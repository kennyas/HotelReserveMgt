using HotelReserveMgt.Core.Application.Configurations;
using HotelReserveMgt.Core.DTOs.Email;
using HotelReserveMgt.Core.Exceptions;
using HotelReserveMgt.Core.Interfaces;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using MailKit.Net.Smtp;
using MailKit.Security;
using System.Text;
using System.Threading.Tasks;

namespace HotelReserveMgt.Infrastructure.Shared.Services
{
    public class EmailService : IEmailService
    {
        public MailConfiguration _mailSettings { get; }
        public ILogger<EmailService> _logger { get; }

        public EmailService(IOptions<MailConfiguration> mailSettings, ILogger<EmailService> logger)
        {
            _mailSettings = mailSettings.Value;
            _logger = logger;
        }

        public async Task SendAsync(EmailRequest request)
        {
            try
            {
                // create message
                var email = new MimeMessage();
                email.Sender = new MailboxAddress(_mailSettings.DisplayName, request.From ?? _mailSettings.From);
                email.To.Add(MailboxAddress.Parse(request.To));
                email.Subject = request.Subject;
                var builder = new BodyBuilder();
                builder.HtmlBody = request.Body;
                email.Body = builder.ToMessageBody();
                using var smtp = new SmtpClient();
                smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.UserName, _mailSettings.Password);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);

            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new ApiException(ex.Message);
            }
        }
    }
}
