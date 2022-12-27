using Core.Entities;
using Microsoft.EntityFrameworkCore;

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