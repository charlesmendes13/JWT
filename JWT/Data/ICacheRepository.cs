﻿using JWT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWT.Data
{
    public interface ICacheRepository
    {
        string Get(string token);

        void Set(RefreshToken refreshToken);
    }
}
