using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class Employee : IdentityUser
    {
        public string Name { get; set; }
        [MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string LastName { get; set; }
        [MaxLength(50)]
        public string Phone { get; set; }
        public List<ProjectActivity> ProjectActivities { get; set; }
        public virtual ICollection<ProjectEmployee> ProjectEmployees { get; set; } = new List<ProjectEmployee>();
        public string ProjectsId { get; set; }
    }
}