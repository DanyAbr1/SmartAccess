﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SmartAccess.Droid.Models
{
    public class OpenRequest
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public string Name { get; set; }
        public DateTime DateRequest { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
        public byte[] Picture { get; set; }
    }
}
