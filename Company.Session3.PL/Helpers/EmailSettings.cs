using System.Net;
using System.Net.Mail;

namespace Company.Session3.PL.Helpers
{
    public static class EmailSettings
    {
        public static bool SendEmail(Email email)
        {
            //Mail Server : Google
            //SMTP

            try
            {
                var client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("noraali18032003@gmail.com", "xadzljbwxyplcmvc");
                client.Send("noraali18032003@gmail.com", email.To, email.Subject, email.Body);

                return true;
            }
            catch (Exception e) 
            {
                return false;
            }
        }
    }
}
