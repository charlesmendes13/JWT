using JWT.Data;
using JWT.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWT.Services
{
    public class UserService : IUserService
    {
        private readonly ITokenService _tokenService;
        private readonly ICacheRepository _cacheRepository;

        public UserService(ITokenService tokenService, 
            ICacheRepository cacheRepository)
        {
            _tokenService = tokenService;
            _cacheRepository = cacheRepository;
        }

        public Token Login(User user)
        {
            if (user.GrantType == "password")
            {
                var userId = "charles.mendes";
                var password = "123";

                if (user.UserId == userId && user.Password == password)
                {
                    return Token(user);
                }
            }
            else if (user.GrantType == "refresh_token")
            {
                var token = _cacheRepository.Get(user.RefreshToken);

                if (!String.IsNullOrWhiteSpace(token))
                {
                    var refreshToken = JsonConvert.DeserializeObject<RefreshToken>(token);

                    if (user.RefreshToken == refreshToken.Token)
                    {
                        return Token(user);
                    }                    
                }                
            }

            return null;
        }

        private Token Token(User user)
        {
            var accessToken = _tokenService.AcessToken(user.UserId);
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
