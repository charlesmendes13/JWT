using System;

namespace JWT.Models
{
    public class RefreshToken
    {
        public string Token { get; set; }
        public DateTime TokenExpires { get; set; }
    }
}
