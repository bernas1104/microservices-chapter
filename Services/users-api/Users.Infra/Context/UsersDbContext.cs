using Microsoft.EntityFrameworkCore;
using Users.Domain.Entities;
using Users.Infra.Mappings;

namespace Users.Infra.Context
{
    public class UsersDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public UsersDbContext(DbContextOptions<UsersDbContext> options)
            : base(options)
        {}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UsersMapping());
        }
    }
}
