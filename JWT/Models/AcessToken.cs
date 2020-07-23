using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWT.Models
{
    public class AcessToken
    {
        public string Token { get; set; }
        public DateTime TokenExpires { get; set; }
    }
}
