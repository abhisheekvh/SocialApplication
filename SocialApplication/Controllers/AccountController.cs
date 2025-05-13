using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialApplication.DTO;
using SocialApplication.Entities;
using SocialApplication.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace SocialApplication.Controllers
{
    
    public class AccountController(DataContext dataContext,ITokenService tokenService) : BaseApiController
    {
        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDto registerDto)
        {
            
            if (!await UserExist(registerDto.Username))
            {
                using var hmac = new HMACSHA512();
                var user = new AppUser
                {
                    UserName = registerDto.Username.ToLower(),
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                    PasswordSalt = hmac.Key

                };
                dataContext.Users.Add(user);
                await dataContext.SaveChangesAsync();

                return new UserDTO
                {
                    Username = user.UserName,
                    Token = tokenService.CreateToken(user)
                };
            }
            return BadRequest("User name is taken!");
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDto loginDto)
        {
            var user = await dataContext.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.userName);
            if( user==null)
            {
                return Unauthorized("Invalid Credentials!");
            }
            using var hmac= new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.password));
            for(int i=0;i<computedHash.Length;i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                {
                    return Unauthorized("Invalid Credentials!");
                }
            }
            return new UserDTO
            {
                Username = user.UserName,
                Token = tokenService.CreateToken(user)
            };
        }
        private async Task<bool> UserExist(string username)
        {
            return await dataContext.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower());
        }
    }
}
