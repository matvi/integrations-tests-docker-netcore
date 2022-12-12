using Microsoft.EntityFrameworkCore;
using TestIntegrations.Entities;

namespace TestIntegrations
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public virtual DbSet<UserEntity> UserEntities { get; set; }

    }
}