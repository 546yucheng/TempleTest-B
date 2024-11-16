using Aprojectbackend.Models;
using SendEmailService.Models;

namespace SendEmailService.Service.EmailService
{
    public interface IEmailService
    {
        //void SendEmail(EmailDto request);
        void SendEmail(EmailDto request, TUser toUser, TUser toUser2);
    }
}
