using Aprojectbackend.DTO.matchDTO;
using Aprojectbackend.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace Aprojectbackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly UserProfileService _userProfileService;
        private readonly UserProfileDisplayService _userProfileDisplayService;

        public UserProfileController(UserProfileService userProfileService, UserProfileDisplayService userProfileDisplayService)
        {
            _userProfileService = userProfileService;
            _userProfileDisplayService = userProfileDisplayService;

        }

        /// <summary>
        /// 取得會員偏好編輯頁(match-edit)
        /// </summary>
        /// /// <returns></returns>
        // GET: api/UserProfile/{userId} - 使用 UserProfileDTO
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserProfile(int userId)
        {
            var userProfileDto = await _userProfileService.GetUserProfileAsync(userId);

            if (userProfileDto == null)
            {
                return NotFound();
            }

            return Ok(userProfileDto);
        }

        /// <summary>
        /// 新增會員偏好編輯頁(match-edit)
        /// </summary>
        /// /// <returns></returns>
        // POST: api/UserProfile - 使用 UserProfileDTO
        [HttpPost]
        public async Task<IActionResult> CreateUserProfile([FromBody] UserProfileDTO userProfileDto)
        {
            if (userProfileDto == null)
            {
                return BadRequest();
            }

            var success = await _userProfileService.CreateUserProfileAsync(userProfileDto);

            if (!success)
            {
                return StatusCode(500, "無法創建用戶資料");
            }

            return CreatedAtAction(nameof(GetUserProfile), new { userId = userProfileDto.UserId }, userProfileDto);
        }

        /// <summary>
        /// 編輯會員偏好編輯頁(match-edit)
        /// </summary>
        /// /// <returns></returns>
        // PUT: api/UserProfile/{userId} - 使用 UserProfileDTO
        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUserProfile(int userId, [FromBody] UserProfileDTO userProfileDto)
        {
            if (userId != userProfileDto.UserId)
            {
                return BadRequest();
            }

            var success = await _userProfileService.UpdateUserProfileAsync(userProfileDto);

            if (!success)
            {
                return StatusCode(500, "無法更新用戶資料");
            }

            return NoContent();
        }

        /// <summary>
        /// 確認會員偏好編輯頁(match-index)
        /// </summary>
        /// /// <returns></returns>
        // GET: api/UserProfile/CheckUserPrefer/{userId} - 使用 UserProfileDTO
        [HttpGet("CheckUserPrefer/{userId}")]
        public async Task<IActionResult> CheckUserPrefer(int userId)
        {
            var userPrefer = await _userProfileService.GetUserPreferByUserIdAsync(userId);

            if (userPrefer == null)
            {
                return NotFound();
            }

            return Ok();
        }

        /// <summary>
        /// 獲取單個配對用戶資料(match)
        /// </summary>
        /// /// <returns></returns>
        // GET: api/UserProfile/Display/{userId} - 使用 UserProfileDisplayDTO
        [HttpGet("Display/{userId}")]
        public ActionResult<UserProfileDisplayDTO> GetUserProfileDisplay(int userId)
        {
            var result = _userProfileDisplayService.GetUserProfileDisplay(userId);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }


        /// <summary>
        /// 獲取配對用戶照片(match-edit)
        /// </summary>
        /// /// <returns></returns>
        // POST: api/UserProfile/upload-photo/{userId}
        [HttpPost("upload-photo/{userId}")]
        public async Task<IActionResult> UploadPhoto(int userId, [FromForm] UploadPhoto filePhoto)
        {
            var file = filePhoto.PhotoFile;

            if (file == null || file.Length == 0)
            {
                return BadRequest("請選擇一張圖片");
            }

            var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadsFolderPath))
            {
                Directory.CreateDirectory(uploadsFolderPath);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(uploadsFolderPath, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var relativePath = Path.Combine("uploads", uniqueFileName).Replace("\\", "/");

            // 更新資料庫中用戶的照片路徑
            var success = await _userProfileService.UpdatePhotoPathAsync(userId, relativePath);
            if (!success)
            {
                return StatusCode(500, "無法更新照片路徑");
            }

            return Ok(new { FilePath = relativePath });
        }

        /// <summary>
        /// 獲取全部配對用戶資料(match)
        /// </summary>
        /// /// <returns></returns>
        // GET: api/UserProfile/DisplayAll - 使用 UserProfileDisplayDTO 返回所有會員資料
        [HttpGet("DisplayAll")]
        public ActionResult<IEnumerable<UserProfileDisplayDTO>> GetAllUserProfiles()
        {
            var result = _userProfileDisplayService.GetAllUserProfiles();
            if (result == null || !result.Any())
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}
