using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JWT.Services;
using JWT.Models;

namespace JWT.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly IUserService _userService;

        public LoginController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] User user)
        {
            var token = _userService.Login(user);

            if (token != null)
            {
                return Ok(new
                {
                    accessToken = token.AcessToken.Token,
                    accessTokenExpires = token.AcessToken.TokenExpires,
                    refreshToken = token.RefreshToken.Token,
                    refreshTokenExpires = token.RefreshToken.TokenExpires
                });
            }

            return BadRequest("Credenciais inválidas...");
        }
    }
}