using Mini_GPT.Models;

namespace Mini_GPT.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser appUser);
    }
}
