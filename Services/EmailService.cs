using EcommerceBackend.IServices;
using System.Net.Mail;
using System.Net;

namespace EcommerceBackend.Services
{
    public class EmailService : IEmailService
    {
        public string SentMail(string email, string subject, string body)
        {
            try
            {
                string fromAddress = "szyrfeeham@gmail.com";
                string toAddress = email;

                MailMessage message = new MailMessage(fromAddress, toAddress);
                message.Subject = subject;
                message.Body = body;

                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential("szyrfeeham@gmail.com", "bapqilfyueegkgvn");
                smtpClient.Send(message);

                return "Email sent!";
            }
            catch (Exception ex)
            {
                return "Error: " + ex;
            }
        }
    }
}
