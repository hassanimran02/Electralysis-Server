﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pleromi.DAL.Entities
{
    public class SignUpRequest
    {
        public string? email { get; set; }
        public string? Password { get; set; }
        public string? phone { get; set; }
    }
}
