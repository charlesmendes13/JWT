using System;

namespace JWT.Models
{
    public class AcessToken
    {
        public string Token { get; set; }
        public DateTime TokenExpires { get; set; }
    }
}
