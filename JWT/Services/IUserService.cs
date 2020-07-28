using JWT.Models;

namespace JWT.Services
{
    public interface IUserService
    {
        bool Verify(User user);
    }
}
