using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistance.Context
{
    public class DataContext : IdentityDbContext<Employee>
    {
        public DataContext(DbContextOptions options) : base(options)
        {       
        }

        public DbSet<Project> Projects { get; set; }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<ProjectActivity> ProjectActivities { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //builder.Entity<ProjectActivity>(x => x.HasKey(aa => new {aa.ProjectId, aa.EmployeeId}));
            
            builder.Entity<ProjectActivity>()
                .HasOne(p => p.Project)
                .WithMany(e => e.Activities)
                .HasForeignKey(aa => aa.ProjectId);

            builder.Entity<ProjectActivity>()
                .HasOne(p => p.Employee)
                .WithMany(e => e.ProjectActivities)
                .HasForeignKey(aa => aa.EmployeeId);
        }
    }
}