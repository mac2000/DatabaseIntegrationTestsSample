using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL
{
    public class EntityFrameworkContext : DbContext
    {
        public EntityFrameworkContext(ConnectionStringSettings connectionStringSettings) : base(connectionStringSettings.ConnectionString) { }

        public DbSet<Post> Posts { get; set; }
    }
}
