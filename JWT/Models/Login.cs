using System;

namespace JWT.Models
{
    public class Login
    {
        public string UserId { get; set; }
        public string Password { get; set; }
        public string RefreshToken { get; set; }
        public string GrantType { get; set; }
    }
}
