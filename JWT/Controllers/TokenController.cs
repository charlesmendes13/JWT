using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using JWT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace JWT.Controllers
{
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IDistributedCache _cache;

        public TokenController(
            IConfiguration configuration, 
            IDistributedCache cache)
        {
            _configuration = configuration;
            _cache = cache;
        }

        [AllowAnonymous]
        [Route("CriarToken")]
        [HttpPost]
        public IActionResult CriarToken([FromBody] User request)
        {
            if (request.UserId == "charles.mendes" && request.AccessKey == "123")
            {
                // JWT Token

                var claims = new[]
                {
                     new Claim(ClaimTypes.Name, request.UserId)
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

                // Gerar RefreshToken

                var refreshToken = new Token
                {
                    UserId = request.UserId,
                    RefreshToken = Guid.NewGuid().ToString().Replace("-", "")                    
                };

                // Salvar RefreshToken

                TimeSpan finalExpiration = TimeSpan.FromMinutes(4);

                DistributedCacheEntryOptions opcoesCache = new DistributedCacheEntryOptions();
                opcoesCache.SetAbsoluteExpiration(finalExpiration);

                _cache.SetString(request.UserId, JsonConvert.SerializeObject(refreshToken), opcoesCache);

                // Retornando

                return Ok(new
                {
                    accessToken = accessToken,
                    accessTokenExpires = token.ValidTo,
                    refreshToken = refreshToken.RefreshToken
                });
            }

            return BadRequest("Credenciais inválidas...");
        }

        [AllowAnonymous]
        [Route("RefreshToken")]
        [HttpPost]
        public IActionResult RefreshToken([FromBody] Token request)
        {
            var strToken = _cache.GetString(request.UserId);

            if (!String.IsNullOrWhiteSpace(strToken))
            {
                var objToken = JsonConvert.DeserializeObject<Token>(strToken);

                if (request.RefreshToken == objToken.RefreshToken)
                {
                    // JWT Token

                    var claims = new[]
                    {
                        new Claim(ClaimTypes.Name, request.UserId)
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

                    // Remover RefreshToken

                    _cache.Remove(request.UserId);

                    // Gerar RefreshToken

                    var refreshToken = new Token
                    {
                        UserId = request.UserId,
                        RefreshToken = Guid.NewGuid().ToString().Replace("-", "")
                    };

                    // Salvar RefreshToken

                    TimeSpan finalExpiration = TimeSpan.FromMinutes(4);

                    DistributedCacheEntryOptions opcoesCache = new DistributedCacheEntryOptions();
                    opcoesCache.SetAbsoluteExpiration(finalExpiration);

                    _cache.SetString(request.UserId, JsonConvert.SerializeObject(refreshToken), opcoesCache);

                    // Retornando

                    return Ok(new
                    {
                        accessToken = accessToken,
                        accessTokenExpires = token.ValidTo,
                        refreshToken = refreshToken.RefreshToken,
                    });
                }
            }            

            return BadRequest("Credenciais inválidas...");
        }
    }
}