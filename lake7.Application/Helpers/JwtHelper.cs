using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using lake7.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace lake7.Application.Helpers
{
    public static class JwtHelper
    {
        public static string GenerateToken(Guid userId, string email, string name, string jwtKey, string issuer, string audience)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Name, name),
                    new Claim("id", userId.ToString()),
                    new Claim("email", email),
                    new Claim("name", name),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(30), // Long-lived token so user doesn't have to login every time
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
