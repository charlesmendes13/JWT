﻿using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using JWT.Models;

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
                Encoding.UTF8.GetBytes(_configuration["JwtToken:SecurityKey"])
                );

            var creds = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
                );

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtToken:Issuer"],
                audience: _configuration["JwtToken:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["JwtToken:TokenExpires"])),
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
            var expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["JwtToken:RefreshTokenExpires"]));

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
