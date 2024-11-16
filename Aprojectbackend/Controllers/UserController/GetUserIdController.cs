using Aprojectbackend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Aprojectbackend.Controllers.UserController
{
    public class GetUserIdController : ControllerBase
    {
        private readonly AprojectContext _context;

        public GetUserIdController(AprojectContext context)
        {
            _context = context;
        }


        // 從 JWT Token 中獲取用戶 ID
        protected async Task<int> GetUserId()
        {
            try
            {
                
                // 檢查用戶是否已認證
                if (User.Identity?.IsAuthenticated == true)
                {
                    // 從 JWT Token 的 Claims 中獲取用戶 email
                    var emailClaim = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");
                    if (emailClaim != null)
                    {
                        // 使用 email 在資料庫中查詢對應的用戶
                        var user = await _context.TUsers
                            .AsNoTracking() 
                            .FirstOrDefaultAsync(u => u.FUserEmail == emailClaim.Value);

                        if (user != null)
                        {
                            return user.FUserId;
                        }
                    }
                }

                return -1;  // 如果未授權或找不到用戶
            }
            catch (Exception)
            {
                return -1;  // 發生異常時返回 -1
            }
        }

        // 檢查授權狀態

        protected async Task<IActionResult> CheckAuthorization()
        {
            var userId = await GetUserId();
            if (userId == -1)
            {
                return Unauthorized(new
                {
                    message = "請先登入",
                    redirectUrl = "http://localhost:4200/user/login"
                });
            }
            return null;
        }
    }
}
