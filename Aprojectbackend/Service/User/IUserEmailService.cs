using System.Threading.Tasks;

namespace Aprojectbackend.Service.User
{
    public interface IUserEmailService
    {
        Task SendVerificationEmailAsync(string to, string verificationLink);
        Task SendPasswordResetEmailAsync(string to, string resetLink);
    }
} 