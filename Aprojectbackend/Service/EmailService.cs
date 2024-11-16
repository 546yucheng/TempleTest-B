using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;
using SendEmailService.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using static Org.BouncyCastle.Math.EC.ECCurve;
using Aprojectbackend.Models;

namespace SendEmailService.Service.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public void SendEmail(EmailDto request, TUser toUser, TUser toUser2)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (string.IsNullOrEmpty(request.To)) throw new ArgumentException("收件人地址不可為空", nameof(request.To));

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailUsername").Value));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;

            // 使用 GenerateEmailBody 方法生成 HTML
            var htmlBody = GenerateEmailBody(toUser, toUser2);

            // 使用 BodyBuilder 組合 HTML 與圖片
            var bodyBuilder = new BodyBuilder { HtmlBody = htmlBody };

            // 添加圖片作為內嵌資源，並設置 ContentId
            var projectRoot = Directory.GetCurrentDirectory(); // 取得專案根目錄
            var imagePath = Path.Combine(projectRoot, "wwwroot", "uploads", "愛打到.gif"); // 組合本地路徑
            var image = bodyBuilder.LinkedResources.Add(imagePath);
            image.ContentId = "EmbeddedImage"; // 與 HTML 中的 cid 相匹配

            // 組合好的內容設為郵件主體
            email.Body = bodyBuilder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(_config["EmailHost"], 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(_config.GetSection("EmailUsername").Value, _config.GetSection("EmailPassword").Value);
            smtp.Send(email);
            smtp.Disconnect(true);
        }

        // 定義 GenerateEmailBody 方法來生成 HTML
        private string GenerateEmailBody(TUser toUser, TUser toUser2)
        {
            string sex = toUser2.FUserGender == false ? "女" : "男";

            return $@"
                <div style='font-family: Arial, sans-serif; text-align: center;'>

                <h2>嗨，{toUser.FUserNickName}！</h2>

                <img src='cid:EmbeddedImage' alt='會員照片' style='width: 150px; height: 150px; object-fit: cover; border-radius: 50%; margin-bottom: 15px;' />
    
                <p>會員 {toUser2.FUserNickName} 對你感興趣，以下是他的資料：</p>
                <p><strong>性別：</strong>{sex}</p>

                <a href='http://localhost:4200/match' style='display: inline-block; padding: 10px 15px; color: white; background-color: #007bff; text-decoration: none; margin-top: 20px;'>立即查看</a>
                </div>";
        }
    }
}
