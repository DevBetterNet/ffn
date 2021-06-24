﻿using System.Collections.Generic;

namespace Dev.Plugin.Sys.Auth.Models
{
    public class AuthResult
    {
        public string Token { get; set; }
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }
}