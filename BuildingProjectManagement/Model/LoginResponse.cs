﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingProjectManagement.Model
{
    public class LoginResponse
    {
        public string? Token { get; set; }
        public DateTime ExpirationTime { get; set; }
    }
}
