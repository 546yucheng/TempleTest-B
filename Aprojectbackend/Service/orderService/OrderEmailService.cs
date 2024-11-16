using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using Aprojectbackend.Service.orderInterface;
using MailKit.Search;
using Aprojectbackend.Models;
using MailKit.Security;
using Microsoft.Extensions.Logging;

namespace Aprojectbackend.Service.orderService
{
    public class OrderEmailService : IOrderEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<OrderEmailService> _logger;

        public OrderEmailService(IConfiguration configuration, ILogger<OrderEmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;

        }

        public async Task SendOrderCompletionEmailAsync(string orderId)
        {
            // 從 appsettings.json 中讀取設定
            var smtpServer = _configuration["EmailHost"];
            var smtpPort = 587; // 使用 Gmail 預設端口
            var email = _configuration["EmailUsername"];
            var password = _configuration["EmailPassword"];

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("廟不可言官方帳號", "moewbookyen@gmail.com"));  // 發送者名稱
            message.To.Add(new MailboxAddress("親愛的會員", "user124michael@gmail.com"));  // 收件人
            message.Subject = "訂單完成通知";


            // 郵件內容
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $@"
            <div style='font-family: Arial, sans-serif; text-align: center;'>
                <h2>您的訂單已完成</h2>
                <p>感謝您的訂購！</p>
                <p>您的訂單編號是：<strong>#{orderId}</strong></p>
                <p>如果有任何問題，請聯繫我們。</p>
            </div>"
            };

            message.Body = bodyBuilder.ToMessageBody();

            // 使用 SmtpClient 發送郵件
            try
            {
                using (var client = new SmtpClient())
                {
                    // 連接到郵件伺服器
                    await client.ConnectAsync(smtpServer, smtpPort, SecureSocketOptions.StartTls);
                    // 認證
                    await client.AuthenticateAsync(email, password);
                    // 發送郵件
                    await client.SendAsync(message);
                    //_logger.LogInformation($"訂單完成通知已成功發送給：{to}"); //如果要寫to收件人的話參數也要改成兩個
                    // 斷開連線
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"發送訂單完成通知郵件失敗：{ex.Message}");
            }
        }
    }
}
