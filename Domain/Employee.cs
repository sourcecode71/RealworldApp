using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class Employee : IdentityUser
    {
        public string Name { get; set; }
        public List<ProjectActivity> ProjectActivities { get; set; }
        public List<Project> Projects { get; set; }
        public string ProjectsId { get; set; }
    }
}