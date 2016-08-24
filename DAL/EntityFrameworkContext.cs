using System.Configuration;
using System.Data.Entity;
using DAL.Models;

namespace DAL
{
    public class EntityFrameworkContext : DbContext
    {
        public EntityFrameworkContext(ConnectionStringSettings connectionStringSettings) : base(connectionStringSettings.ConnectionString) { }

        public DbSet<Post> Posts { get; set; }
    }
}
