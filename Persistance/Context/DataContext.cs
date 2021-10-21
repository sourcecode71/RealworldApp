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
    }
}