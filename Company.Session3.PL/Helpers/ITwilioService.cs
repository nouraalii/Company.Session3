
using Company.Session3.DAL.SMS;
using Twilio.Rest.Api.V2010.Account;

namespace Company.Session3.PL.Helpers
{
    public interface ITwilioService
    {
        public MessageResource SendSMS(SMS Sms);
    }
}
