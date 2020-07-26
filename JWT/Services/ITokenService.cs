using JWT.Models;

namespace JWT.Services
{
    public interface ITokenService
    {
        AcessToken AcessToken(string userId);

        RefreshToken RefreshToken();
    }
}
