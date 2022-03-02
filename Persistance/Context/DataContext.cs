using Domain;
using Domain.Common;
using Domain.Projects;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistance.Context
{
    public class DataContext : IdentityDbContext<Employee>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {       
        }

        public DbSet<Project> Projects { get; set; }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<ProjectActivity> ProjectActivities { get; set; }
        public DbSet<ProjectEmployee> ProjectEmployees { get; set; }
        public DbSet<ProjectBudgetActivities> ProjectBudgetActivities { get; set; }
        public DbSet<WorkOrder> WorkOrder { get; set; }
        public DbSet<HisWorkOrder> HisWorkOrder { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<Invoice> Invoice { get; set; }
        public DbSet<Hourlogs> Hourlogs { get; set; }
        public DbSet<ProdStatus> ProdStatus { get; set; }
        public DbSet<ProjectsStatus> ProjectsStatus { get; set; }

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