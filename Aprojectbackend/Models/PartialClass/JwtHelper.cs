using Aprojectbackend.DTO.UserDTO;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Aprojectbackend.Models.PartialClass
{
    public static class JwtHelper
    {
        public static string GenerateJwtToken(this TUser user, JwtSettings settings)
        {
            if (string.IsNullOrEmpty(settings.Key) || settings.Key.Length < 32)
            {
                throw new ArgumentException("JWT key must be at least 32 characters long");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(settings.Key);
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("email", user.FUserEmail),
                    new Claim("id", user.FUserId.ToString()),
                    new Claim("name", user.FUserName)
                }),
                Expires = DateTime.UtcNow.AddDays(settings.ExpiryInDays),
                Issuer = "https://localhost:7203",  // ½T«O»P appsettings.json ¤@­P
                Audience = "https://localhost:4200",
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
} 