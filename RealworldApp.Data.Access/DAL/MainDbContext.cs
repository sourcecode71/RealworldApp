using Microsoft.EntityFrameworkCore;
using RealworldApp.Data.Model.Listings;

namespace RealworldApp.Data.Access.DAL
{
    public class MainDbContext : DbContext
    {
        public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
        {
        }

        public DbSet<Brand> Brand { get; set; }

        public DbSet<Category> Category { get; set; }
        public DbSet<Size> Size { get; set; }
        public DbSet<SubCategory> SubCategory { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

    }
}
