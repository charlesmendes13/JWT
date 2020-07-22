using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWT.Models
{
    public class Token
    {
        public string UserId { get; set; }
        public string RefreshToken { get; set; }        
    }
}
