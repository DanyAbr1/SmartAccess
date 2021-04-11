using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartAccess.Api.Models
{
    public class RpiStatus
    {
        public int Id { get; set; }
        public string TimeUp { get; set; }
        public string Temperature { get; set; }
        public string Email { get; set; }
    }
}
