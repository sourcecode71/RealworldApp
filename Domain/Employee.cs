using System.Collections.Generic;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class Employee : IdentityUser
    {
        public string Name { get; set; }
        public List<ProjectActivity> ProjectActivities { get; set; }
        public virtual ICollection<ProjectEmployee> ProjectEmployees { get; set; } = new List<ProjectEmployee>();
        public string ProjectsId { get; set; }
    }
}