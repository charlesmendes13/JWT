using JWT.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace JWT.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public AcessToken AcessToken(string userId)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, userId)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["SecurityKey"])
                );

            var creds = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
                );

            var token = new JwtSecurityToken(
                issuer: _configuration["iss"],
                audience: _configuration["aud"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(2),
                signingCredentials: creds
                );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            return new AcessToken
            { 
                Token = accessToken,
                TokenExpires = token.ValidTo
            };
        }

        public RefreshToken RefreshToken()
        {
            var randomNumber = new byte[32];
            var expires = DateTime.UtcNow.AddMinutes(4);

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);

                return new RefreshToken
                {
                    Token = Convert.ToBase64String(randomNumber),
                    TokenExpires = expires
                };                    
            }
        }
    }
}
