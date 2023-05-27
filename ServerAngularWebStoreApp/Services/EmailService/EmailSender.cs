using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;

namespace Services.EmailService
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _emailConfig;

        public EmailSender(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }

        public async Task SendEmailAsync(Message message)
        {
            var emailMessage = CreateEmailMessage(message);

            await Send(emailMessage);
        }

        private async Task Send(MailMessage mailMessage)
        {
            using (var client = new SmtpClient(_emailConfig.SmtpServer, _emailConfig.Port))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(_emailConfig.UserName, _emailConfig.Password);

                try
                {
                    await client.SendMailAsync(mailMessage);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    //client.Disconnect(true);
                    client.Dispose();
                }
            }
        }

        private MailMessage CreateEmailMessage(Message message)
        {
            var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(_emailConfig.From); // From is a string
            foreach (var recipient in message.To)
            {
                mailMessage.To.Add(new MailAddress(recipient.Address)); // recipient should be string
            }
            mailMessage.Subject = message.Subject;
            mailMessage.Body = message.Content;

            return mailMessage;
        }
    }

}