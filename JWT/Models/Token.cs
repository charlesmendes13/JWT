﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWT.Models
{
    public class Token
    {
        public AcessToken AcessToken { get; set; }
        public RefreshToken RefreshToken { get; set; }
    }
}