using System.Linq;
using Microsoft.AspNetCore.Identity;
using JWT.Models;
using JWT.Extensions;

namespace JWT.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;

        public UserService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public bool Verify(User user)
        {
            var result = _userManager.Users.FirstOrDefault(x =>
                x.UserName.Equals(user.UserName));

            var verify = HasherExtension.VerifyHashedPassword(result.PasswordHash, user.PasswordHash);

            if (verify)
            {
                return true;
            }

            return false;
        }
    }
}
