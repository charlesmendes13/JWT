using System;

namespace JWT.Models
{
    public class Token
    {
        public AcessToken AcessToken { get; set; }
        public RefreshToken RefreshToken { get; set; }
    }
}
