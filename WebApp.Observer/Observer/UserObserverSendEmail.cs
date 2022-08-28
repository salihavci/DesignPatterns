using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Mail;
using WebApp.Ovserver.Models;

namespace WebApp.Observer.Observer
{
    public class UserObserverSendEmail : IUserObserver
    {
        private readonly IServiceProvider _serviceProvider;

        public UserObserverSendEmail(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void UserCreated(AppUser appUser)
        {
            var logger = _serviceProvider.GetRequiredService<ILogger<UserObserverSendEmail>>();
            var mailMessage = new MailMessage();
            var client = new SmtpClient("mail.salihavci.com");
            mailMessage.From = new MailAddress("info@salihavci.com");
            mailMessage.To.Add(new MailAddress(appUser.Email));
            mailMessage.Subject = "Sitemize Hoşgeldiniz.";
            mailMessage.Body = "<p>Sitemizin genel kuralları : deneme kurallar</p>";
            mailMessage.IsBodyHtml = true;
            client.Port = 587;

            client.Credentials = new NetworkCredential("info@salihavci.com", "P4SSW0RD");
            client.Send(mailMessage);
            logger.LogInformation($"Email was send to user : {appUser.UserName}.");
        }
    }
}
