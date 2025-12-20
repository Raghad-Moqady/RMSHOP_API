using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace RMSHOP.BLL.Service
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("raghadmoqady@gmail.com", "aoab gshf upwb gvqr")
            };

            return client.SendMailAsync(
                new MailMessage(from: "raghadmoqady@gmail.com",
                                to: email,
                                subject,
                                htmlMessage            
                                )
                { IsBodyHtml=true});
        }
    
    }
}
