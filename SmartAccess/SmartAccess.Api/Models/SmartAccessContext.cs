using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartAccess.Api.Models
{
    public partial class SmartAccessContext:DbContext
    {
        public SmartAccessContext(DbContextOptions<SmartAccessContext> options)
            :base(options)
        {

        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Chat> Chats { get; set; }
        public virtual DbSet<OpenRequest>  OpenRequests{ get; set; }
        public virtual DbSet<RpiStatus> Rpis{ get; set; }
    }
}
