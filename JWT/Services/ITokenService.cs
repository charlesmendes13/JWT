﻿using JWT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWT.Services
{
    public interface ITokenService
    {
        AcessToken AcessToken(string userId);

        RefreshToken RefreshToken();
    }
}
