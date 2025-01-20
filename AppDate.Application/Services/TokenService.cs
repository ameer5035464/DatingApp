using AppDate.Application.Abstraction.Services;
using AppDate.Domain.Entities.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AppDate.Application.Services
{
    public class TokenService(IConfiguration config) : ITokenService
    {

        public string CreateToken(AppUser appUser)
        {
            var tokenKey = config["TokenKey"];

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey!));

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,appUser.UserName)
            };

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDesc = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDesc);
            return tokenHandler.WriteToken(token);
        }
    }
}
