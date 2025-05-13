using SocialApplication.Entities;

namespace SocialApplication.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser appUser);
    }
}
