using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using lake7.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace lake7.Application.Helpers
{
    public static class JwtHelper
    {
        public static string GenerateToken(Guid userId,string email, string jwtKey, string issuer, string audience)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()), // ✅ REQUIRED
                    new Claim(ClaimTypes.Email, email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
