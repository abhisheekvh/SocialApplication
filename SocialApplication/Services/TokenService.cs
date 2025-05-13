using Microsoft.IdentityModel.Tokens;
using SocialApplication.Entities;
using SocialApplication.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SocialApplication.Services
{
    public class TokenService(IConfiguration config) : ITokenService
    {
        public string CreateToken(AppUser appUser)
        {
            var tokenKey = config["TokenKey"] ?? throw new Exception("Can't access token key from appsettings");
            if(tokenKey.Length<64)
            {
                throw new Exception("Your token key needs to be longer");
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
            var claims = new List<Claim>
            {

                new(ClaimTypes.Name,appUser.UserName)
            };
            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = cred
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(descriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
