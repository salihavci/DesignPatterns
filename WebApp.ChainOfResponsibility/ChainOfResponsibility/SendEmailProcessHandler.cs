using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace WebApp.ChainOfResponsibility.ChainOfResponsibility
{
    public class SendEmailProcessHandler:ProcessHandler
    {
        private readonly string _fileName;
        private readonly string _toEmail;

        public SendEmailProcessHandler(string fileName, string toEmail)
        {
            _fileName = fileName;
            _toEmail = toEmail;
        }

        public override object Handle(object value)
        {
            var zipMemoryStream = value as MemoryStream;
            zipMemoryStream.Position = 0;
            var mailMessage = new MailMessage();
            var smtpClient = new SmtpClient("mail.salihavci.com");
            mailMessage.From = new MailAddress("info@salihavci.com");
            mailMessage.To.Add(new MailAddress(_toEmail));
            mailMessage.Subject = "Zip dosyası.";
            mailMessage.Body = "<p>Zip dosyası ektedir.</p>";
            Attachment attachment = new Attachment(zipMemoryStream, _fileName, MediaTypeNames.Application.Zip);
            mailMessage.Attachments.Add(attachment);
            mailMessage.IsBodyHtml = true;
            smtpClient.Port = 587;
            smtpClient.Credentials = new NetworkCredential("info@salihavci.com", "P4SSW0RD");
            smtpClient.Send(mailMessage);


            return base.Handle(value);
        }
    }
}
