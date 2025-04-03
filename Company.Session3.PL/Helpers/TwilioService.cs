using Company.Session3.DAL.SMS;
using Company.Session3.PL.WorkshopSettings;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Company.Session3.PL.Helpers
{
    public class TwilioService(IOptions<TwilioSettings> _options) : ITwilioService
    {


        public MessageResource SendSMS(SMS Sms)
        {
            //Intialize Connection 
            TwilioClient.Init(_options.Value.AccountSID,_options.Value.AuthToken);

            //Build Message
            var meesage = MessageResource.Create(
                body:Sms.Body,
                to:Sms.To,
                from:_options.Value.PhoneNumber

                );

            //return Message
            return meesage;
        }
    }
}
