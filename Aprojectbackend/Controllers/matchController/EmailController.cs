using Aprojectbackend.DTO.matchDTO;
using Aprojectbackend.Models;
using MailKit.Net.Smtp;  // 確認使用 MailKit 的 SmtpClient
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;
using SendEmailService.Models;


namespace SendEmailService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly AprojectContext _context; // 假設你有一個資料庫上下文
        public EmailController(IEmailService emailService, AprojectContext context) 
        {
            _emailService = emailService;
            _context = context;
        }

        /// <summary>
        /// 寄感興趣通知信(match popup)
        /// </summary>
        /// /// <returns></returns>
        [HttpPost("send-interest")]
        public IActionResult SendInterestNotification([FromBody] InterestRequestDto request)
        {
            if (request == null || request.ToUserId <= 0 || request.FromUserId <= 0)
            {
                return BadRequest("無效的請求");
            }

            // 從資料庫中撈取收信人 (會員B) 的資料
            var toUser = _context.TUsers.FirstOrDefault(u => u.FUserId == request.ToUserId);
            if (toUser == null)
            {
                return NotFound("收信人資料不存在");
            }

            // 從資料庫中撈取發信人 (會員A) 的偏好資料
            var fromUserPrefer = _context.TUsers.FirstOrDefault(p => p.FUserId == request.FromUserId);
            if (fromUserPrefer == null)
            {
                return NotFound("發信人資料不存在");
            }

            // 準備信件內容
            var emailRequest = new EmailDto
            {
                To = toUser.FUserEmail,
                Subject = "有會員對你感興趣！",
            };

            // 傳遞 toUser 和 fromUserPrefer 到 SendEmail 方法
            _emailService.SendEmail(emailRequest, toUser, fromUserPrefer);

            return Ok("信件已成功發送");
        }
    }
}
