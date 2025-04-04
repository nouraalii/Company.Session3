using Company.Session3.PL.WorkshopSettings;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Company.Session3.PL.Helpers
{
    public class MailService : IMailService
    {
        private readonly MailSettings _options;

        public MailService(IOptions<MailSettings> options)
        {
            _options = options.Value;
        }

        public void SendEmail(Email email)
        {
            //Build Message
            var mail = new MimeMessage();

            mail.Subject = email.Subject;
            mail.From.Add(new MailboxAddress(_options.DisplayName,_options.Email));
            mail.To.Add(MailboxAddress.Parse(email.To));

            var builder = new BodyBuilder();
            builder.TextBody = email.Body;
            mail.Body=builder.ToMessageBody();

            //Establish Connection

            using var smtp = new SmtpClient();
            smtp.Connect(_options.Host,_options.Port,MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(_options.Email,_options.Password);

            //Send Message
            smtp.Send(mail);
        }
    }
}
