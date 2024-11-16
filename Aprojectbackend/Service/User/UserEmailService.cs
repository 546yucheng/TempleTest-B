using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Aprojectbackend.Service.User
{
    /// <summary>
    /// 電子郵件服務
    /// </summary>
    public class UserEmailService : IUserEmailService
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="configuration">配置注入</param>
        public UserEmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// 發送驗證郵件
        /// </summary>
        /// <param name="to">收件者信箱</param>
        /// <param name="verificationLink">驗證連結</param>
        public async Task SendVerificationEmailAsync(string to, string verificationLink)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("focus911209@gmail.com", "iacr nnpn alal fpkw"),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("focus911209@gmail.com"),
                Subject = "會員註冊驗證",
                Body = $@"
                        <div style='font-family: Arial, sans-serif; text-align: center;'>                            
                        <h2>感謝您的註冊</h2>
                        <p>請點擊以下連結完成驗證：</p>
                        <a href='{verificationLink}' style='display: inline-block; padding: 10px 15px; color: white; background-color: #007bff; text-decoration: none; margin-top: 20px;'>驗證帳號</a>
                        </ div > ",


                IsBodyHtml = true
            };
            mailMessage.To.Add(to);

            await smtpClient.SendMailAsync(mailMessage);
        }

        /// <summary>
        /// 發送重設密碼郵件
        /// </summary>
        /// <param name="to">收件者信箱</param>
        /// <param name="resetLink">重設密碼連結</param>
        public async Task SendPasswordResetEmailAsync(string to, string resetLink)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("focus911209@gmail.com", "iacr nnpn alal fpkw"),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("focus911209@gmail.com"),
                Subject = "重設密碼請求",
                Body = $@"
                        <div style='font-family: Arial, sans-serif; text-align: center;'>
                        <h2>重設密碼</h2>
                        <p>請點擊以下連結重設您的密碼：</p>
                        <p>此連結將在1分鐘後失效</p>
                        <a href='{resetLink}' style='display: inline-block; padding: 10px 15px; color: white; background-color: #007bff; text-decoration: none; margin-top: 20px;'>重設密碼</a>
                        </div>",
                IsBodyHtml = true
            };
            mailMessage.To.Add(to);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
} 