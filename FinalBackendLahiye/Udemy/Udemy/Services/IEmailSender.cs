﻿using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;

namespace Udemy.Services
{
    public interface IEmailSender
    {

        void SendEmail(string to,string subject,string text);   

    }

    public class EmailSender:IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void SendEmail(string to, string subject, string text)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_configuration["Email:Email"]));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = text };

            using (var smtp = new SmtpClient())
            {
                smtp.Connect(_configuration["Email:Server"], Convert.ToInt32(_configuration["Email:Port"]), true);
                smtp.Authenticate(_configuration["Email:Email"], _configuration["Email:Password"]);
                smtp.Send(email);
                smtp.Disconnect(true);
            }
        }
    }
}
