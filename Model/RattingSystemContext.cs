using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RattingSystem.Model.Common;

namespace RattingSystem.Model
{
    public class RattingSystemContext : IdentityDbContext<IdentityUser>
    {
        public RattingSystemContext()
        {
        }
        public RattingSystemContext(DbContextOptions<RattingSystemContext> options)
            : base(options)
        {
        }
        public virtual DbSet<IdentityUser> IdentityUser { get; set; }
        public virtual DbSet<Session> Session { get; set; }
        public virtual DbSet<Rate> Rate { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            AppConfiguration appConfiguration = new AppConfiguration();
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(appConfiguration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData(
               new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
               new IdentityRole { Id = "2", Name = "User", NormalizedName = "USER" }
               );
            base.OnModelCreating(modelBuilder);
        }
    }
}
