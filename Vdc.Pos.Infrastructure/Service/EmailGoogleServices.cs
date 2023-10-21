using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Vdc.Pos.Infrastructure.Service.Interfaces;
using Vdc.Pos.Infrastructure.Settings;

namespace Vdc.Pos.Infrastructure.Service
{
    public class EmailGoogleServices : IEmailService
    { 
        private readonly EmailSmtpSettings _emailSmtpSettings;
        public EmailGoogleServices(IOptions<EmailSmtpSettings> emailSmtpSettings) 
        {
            _emailSmtpSettings = emailSmtpSettings.Value;
        }

        public void SendEmail(string from, string to, string sub, string body)
        {
            try
            {

                string fromAddress = from;
                string toAddress = to;
                string subject = sub;
                string bodyOfEmail = body;

                SmtpClient smtpClient = new SmtpClient(_emailSmtpSettings.Smtp)
                {
                    Port = _emailSmtpSettings.Port,
                    Credentials = new NetworkCredential(_emailSmtpSettings.Email, _emailSmtpSettings.Secret),
                    EnableSsl = true,
                };

                MailMessage mailMessage = new MailMessage(fromAddress, toAddress);

                mailMessage.Subject = subject;
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = body;
                
                smtpClient.Send(mailMessage);
                
            }
             catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}
