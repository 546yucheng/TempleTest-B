using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Aprojectbackend.Models;
using Aprojectbackend.DTO.UserDTO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;

namespace Aprojectbackend.Controllers.UserController
{
    [Route("api/[controller]")]
    [ApiController]
    public class TUsersPersonelinfoController : GetUserIdController
    {
        private readonly AprojectContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly IMemoryCache _cache;
        private readonly IPasswordService _passwordService;

        public TUsersPersonelinfoController(AprojectContext context, IWebHostEnvironment environment) : base(context)
        {
            _context = context;
            _environment = environment;
        }

        // GET: api/TUsersPersonelinfo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TUser>>> GetTUsers()
        {
            return await _context.TUsers.ToListAsync();
        }

        // GET: api/TUsersPersonelinfo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TUser>> GetTUser(int id)
        {
            try
            {
                var tUser = await _context.TUsers
                    .Include(u => u.FUserState)
                    .FirstOrDefaultAsync(u => u.FUserId == id);

                if (tUser == null)
                {
                    return NotFound();
                }
                
                var userDto = new UserPersonelinfo
                {
                    FUserEmail = tUser.FUserEmail,
                    FUserName = tUser.FUserName,
                    FUserPhone = tUser.FUserPhone,
                    FUserGender = tUser.FUserGender,
                    FUserAddress = tUser.FUserAddress,
                    FUserBirthdate = tUser.FUserBirthdate,
                    FUserNickName = string.IsNullOrEmpty(tUser.FUserNickName) ? tUser.FUserName : tUser.FUserNickName,
                    FUserState = tUser.FUserState?.FUserState ?? "未知狀態",
                    FUserStateId = tUser.FUserStateId,
                    FUserImage = tUser.FUserImage
                };

                return Ok(userDto);
            }
            catch (Exception ex)
            {
                // 添加信息來追踪錯誤
                Console.WriteLine($"Error fetching user details: {ex.Message}");
                return StatusCode(500, "伺服器錯誤，請稍後再試");
            }
        }

        [HttpPost("update/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromForm] UserPersonelinfoEdit userEdit)
        {
            try
            {
                var user = await _context.TUsers.FindAsync(id);
                if (user == null)
                {
                    return NotFound("找不到該使用者");
                }

                // 處理照片上傳
                if (userEdit.Photo != null)
                {
                    // 確保上傳資料夾存在
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "users");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // 生成唯一的檔案名稱
                    var fileExtension = Path.GetExtension(userEdit.Photo.FileName);
                    var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // 儲存檔案
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await userEdit.Photo.CopyToAsync(stream);
                    }

                    // 儲存相對路徑到資料庫
                    user.FUserImage = $"/uploads/users/{uniqueFileName}";
                }

                // 更新其他欄位
                if (!string.IsNullOrEmpty(userEdit.FUserName))
                    user.FUserName = userEdit.FUserName;
                if (!string.IsNullOrEmpty(userEdit.FUserPhone))
                    user.FUserPhone = userEdit.FUserPhone;
                if (!string.IsNullOrEmpty(userEdit.FUserAddress))
                    user.FUserAddress = userEdit.FUserAddress;
                if (!string.IsNullOrEmpty(userEdit.FUserNickName))
                    user.FUserNickName = userEdit.FUserNickName;

                // 性別可以是 null
                user.FUserGender = userEdit.FUserGender;

                // 處理生日
                if (!string.IsNullOrEmpty(userEdit.FUserBirthdate))
                {
                    if (DateTime.TryParse(userEdit.FUserBirthdate, out DateTime birthDate))
                    {
                        user.FUserBirthdate = DateOnly.FromDateTime(birthDate);
                    }
                }

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    photoPath = user.FUserImage,
                    message = "更新成功"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "更新失敗",
                    error = ex.Message
                });
            }
        }

        // PUT: api/TUsersPersonelinfo/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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

        // POST: api/TUsersPersonelinfo
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TUser>> PostTUser(TUser tUser)
        {
            _context.TUsers.Add(tUser);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTUser", new { id = tUser.FUserId }, tUser);
        }

        // DELETE: api/TUsersPersonelinfo/5
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
    }
}
