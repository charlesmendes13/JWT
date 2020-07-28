using JWT.Models;

namespace JWT.Data
{
    public interface ICacheRepository
    {
        RefreshToken Get(string token);

        void Set(RefreshToken refreshToken);
    }
}
