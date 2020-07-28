using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JWT.Services;
using JWT.Models;

namespace JWT.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login([FromBody] Login login)
        {
            var token = _loginService.Login(login);

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