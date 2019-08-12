using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using JWT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace JWT.Controllers
{
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        private readonly IConfiguration _configuration;
        public TokenController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Token([FromBody] Usuario request)
        {
            if (request.email == "charlesluizmendes@gmail.com" && request.senha == "123")
            {
                var claims = new[]
                {
                     new Claim(ClaimTypes.Name, request.email)
                };

                var key = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_configuration["SecurityKey"])
                    );

                var creds = new SigningCredentials(
                    key, SecurityAlgorithms.HmacSha256
                    );

                var token = new JwtSecurityToken(
                    issuer: _configuration["iss"],
                    audience: _configuration["aud"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),                   
                    signingCredentials: creds
                    );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                return Ok(tokenString);
            }

            return BadRequest("Credenciais inválidas...");
        }
    }
}