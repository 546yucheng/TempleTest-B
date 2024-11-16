using Aprojectbackend.DTO.UserDTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Aprojectbackend.Models.PartialClass
{
    public class UserContextMiddleware
    {
        private readonly RequestDelegate _next;

        public UserContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, AprojectContext dbContext)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                var userEmail = context.User.Claims.FirstOrDefault(x => x.Type == "email")?.Value;
                if (userEmail != null)
                {
                    var user = await dbContext.TUsers.FirstOrDefaultAsync(u => u.FUserEmail == userEmail);
                    if (user != null)
                    {
                        context.Items["User"] = user;
                    }
                }
            }
            await _next(context);
        }
    }
}
