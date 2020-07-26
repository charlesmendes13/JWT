using JWT.Data;
using JWT.Models;

namespace JWT.Services
{
    public class LoginService : ILoginService
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly ICacheRepository _cacheRepository;

        public LoginService(
            IUserService userService,
            ITokenService tokenService, 
            ICacheRepository cacheRepository)
        {
            _userService = userService;
            _tokenService = tokenService;
            _cacheRepository = cacheRepository;
        }

        public Token Login(Login login)
        {
            if (login.GrantType == "password")
            {
                var user = new User
                {
                    UserName = login.UserId,
                    PasswordHash = login.Password 
                };

                var verify = _userService.Verify(user);

                if (verify)
                {
                    return Token(login);
                }
            }
            else if (login.GrantType == "refresh_token")
            {
                var refreshToken = _cacheRepository.Get(login.RefreshToken);

                if (refreshToken != null)
                {
                    return Token(login);
                }             
            }

            return null;
        }

        private Token Token(Login login)
        {
            var accessToken = _tokenService.AcessToken(login.UserId);
            var refreshToken = _tokenService.RefreshToken();
                        
            _cacheRepository.Set(refreshToken);

            return new Token
            {
                AcessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
    }
}
