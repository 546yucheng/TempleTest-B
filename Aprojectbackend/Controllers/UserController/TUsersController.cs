using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Aprojectbackend.Models;
using Aprojectbackend.DTO.UserDTO;
using Microsoft.AspNetCore.Cors;
using Aprojectbackend.Service;
using Aprojectbackend.Service.User;
using Microsoft.Extensions.Caching.Memory;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using XAct.Users;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Aprojectbackend.Models.PartialClass;

namespace Aprojectbackend.Controllers.UserController
{
    /// <summary>
    /// 使用者控制器
    /// </summary>
    [Authorize] // 需要帶TOKEN方能呼叫
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("All")]
    public class TUsersController : GetUserIdController
    {
        private readonly AprojectContext _context;
        private readonly IUserEmailService _emailService;
        private readonly IMemoryCache _cache;
        private readonly IPasswordService _passwordService;
        private readonly IConfiguration _configuration;
        /// <summary>
        /// 建構子
        /// </summary>
        public TUsersController(
            AprojectContext context,
            IUserEmailService emailService,
            IMemoryCache cache,
            IPasswordService passwordService,
            IConfiguration configuration) : base(context)
        {
            _context = context;
            _emailService = emailService;
            _cache = cache;
            _passwordService = passwordService;
            _configuration = configuration;
        }

        /// <summary>
        /// 註冊新用戶
        /// </summary>
        [AllowAnonymous]// 允許沒有token呼叫
        [HttpPost("signup")]
        //[Authorize]
        public async Task<ActionResult<TUser>> Signup(TUser tUser)
        {
            try
            {

                // 檢查信箱是否已存在
                if (await _context.TUsers.AnyAsync(u => u.FUserEmail == tUser.FUserEmail))
                {
                    return Conflict(new { message = "此信箱已註冊過" }); // 回傳 409 錯誤
                }

                // 使用密碼服務來處理密碼
                var (hashedPassword, salt) = _passwordService.HashPassword(tUser.FUserPassword);
                tUser.FUserPassword = hashedPassword;
                tUser.FUserPasswordSalt = salt;

                tUser.FUserStateId = 2; // 未驗證狀態
                _context.TUsers.Add(tUser);
                await _context.SaveChangesAsync();

                // 產生 JWT Token
                var jwtSettings = new JwtSettings
                {
                    Key = _configuration["JwtSettings:Key"],
                    Issuer = _configuration["JwtSettings:Issuer"],
                    Audience = _configuration["JwtSettings:Audience"],
                    ExpiryInDays = int.Parse(_configuration["JwtSettings:ExpiryInDays"])
                };
                var token = tUser.GenerateJwtToken(jwtSettings);


                // 修改驗證連結，使用正確的路徑
                var verificationLink = $"http://localhost:4200/user/personelinfo?verify=true&userId={tUser.FUserId}";
                await _emailService.SendVerificationEmailAsync(tUser.FUserEmail, verificationLink);

                return Ok(new { message = "註冊成功", userId = tUser.FUserId, token = token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "註冊失敗", error = ex.Message });
            }
        }

        /// <summary>
        /// 用戶登入
        /// </summary>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<TUser>> Login([FromBody] UserLogin loginDTO)
        {
            try
            {
                // 查找使用者
                var user = await _context.TUsers
                .FirstOrDefaultAsync(u => u.FUserEmail == loginDTO.FUserEmail);

                // 檢查帳號是否存在
                if (user == null)
                {
                    return NotFound(new { message = "查無此帳號，請先註冊" });
                }
                // 驗證密碼
                if (!_passwordService.VerifyPassword(
                    loginDTO.FUserPassword,
                    user.FUserPasswordSalt,
                    user.FUserPassword))
                {
                    return Unauthorized(new { message = "輸入密碼不正確" });
                }

                // 產生 JWT Token
                var jwtSettings = new JwtSettings
                {
                    Key = _configuration["JwtSettings:Key"],
                    Issuer = _configuration["JwtSettings:Issuer"],
                    Audience = _configuration["JwtSettings:Audience"],
                    ExpiryInDays = int.Parse(_configuration["JwtSettings:ExpiryInDays"])
                };
                var token = user.GenerateJwtToken(jwtSettings);

                return Ok(new { message = "登入成功", userId = user.FUserId, token = token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "登入失敗", error = ex.Message });
            }
        }

        /// <summary>
        /// 取得用戶資料
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<TUser>> GetTUser(int id)
        {
            var tUser = await _context.TUsers.FindAsync(id);

            if (tUser == null)
            {
                return NotFound();
            }

            return tUser;
        }

        /// <summary>
        /// 更新用戶資料
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTUser(int id, TUser tUser)
        {
            if (id != tUser.FUserId)
            {
                return BadRequest();
            }

            _context.Entry(tUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TUserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// 新增用戶
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<TUser>> PostTUser(TUser tUser)
        {
            _context.TUsers.Add(tUser);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTUser", new { id = tUser.FUserId }, tUser);
        }

        /// <summary>
        /// 刪除用戶
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTUser(int id)
        {
            var tUser = await _context.TUsers.FindAsync(id);
            if (tUser == null)
            {
                return NotFound();
            }

            _context.TUsers.Remove(tUser);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TUserExists(int id)
        {
            return _context.TUsers.Any(e => e.FUserId == id);
        }

        /// <summary>
        /// 驗證用戶信箱
        /// </summary>
        [HttpPost("verify-email/{userId}")]
        public async Task<IActionResult> VerifyEmail(int userId)
        {
            try
            {
                var user = await _context.TUsers
                    .Include(u => u.FUserState)
                    .FirstOrDefaultAsync(u => u.FUserId == userId);

                if (user == null)
                {
                    return NotFound("找不到用戶");
                }

                user.FUserStateId = 1; // 更新為已驗證狀態
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "驗證成功",
                    userId = user.FUserId,
                    stateId = user.FUserStateId,
                    state = user.FUserState?.FUserState
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "驗證失敗", error = ex.Message });
            }
        }

        // 處理忘記密碼API
        [AllowAnonymous]// 允許沒有token呼叫
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDTO request)
        {
            // 檢查信箱是否存在於資料庫
            var user = await _context.TUsers.FirstOrDefaultAsync(u => u.FUserEmail == request.Email);
            if (user == null)
            {
                return NotFound(new { message = "此信箱未註冊，請先註冊會員" });
            }

            // 生成重設密碼的token
            var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            // 將token和會員ID存入快取，設定 1 分鐘後過期
            _cache.Set($"pwreset_{token}", user.FUserId, TimeSpan.FromMinutes(1));
            // 建立重設密碼的連結
            var resetLink = $"http://localhost:4200/user/resetpassword?token={token}&email={request.Email}";

            try
            {
                // 發送重設密碼郵件
                await _emailService.SendPasswordResetEmailAsync(request.Email, resetLink);
                return Ok(new { message = "重設密碼郵件已發送，請查收" });
            }
            catch
            {
                return StatusCode(500, new { message = "發送郵件時發生錯誤" });
            }
        }

        // 處理重設密碼API
        [AllowAnonymous]// 允許沒有token呼叫
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO request)
        {
            // 驗證新密碼與確認密碼是否相符
            if (request.NewPassword != request.ConfirmPassword)
            {
                return BadRequest(new { message = "密碼與確認密碼不符" });
            }

            // 從token中取得會員ID
            var userId = _cache.Get<int?>($"pwreset_{request.Token}");
            if (!userId.HasValue)
            {
                return BadRequest(new { message = "重設連結已過期或無效" });
            }

            // 確認會員有無
            var user = await _context.TUsers.FindAsync(userId.Value);
            if (user == null)
            {
                return NotFound(new { message = "找不到使用者" });
            }

            // 使用密碼服務更新密碼
            var (hashedPassword, salt) = _passwordService.HashPassword(request.NewPassword);
            user.FUserPassword = hashedPassword;
            user.FUserPasswordSalt = salt;

            await _context.SaveChangesAsync();
            // 移除快取中的token
            _cache.Remove($"pwreset_{request.Token}");

            return Ok(new { message = "密碼重設成功" });
        }
    }
}
