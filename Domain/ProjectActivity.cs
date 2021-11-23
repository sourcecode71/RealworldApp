using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Enums;

namespace Domain
{
    public class ProjectActivity
    {
        public string Id { get; set; }
        public Project Project { get; set; }
        public string ProjectId { get; set; }
        public int SelfProjectId { get; set; }
        public Employee Employee { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeEmail { get; set; }
        public double Duration { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        public string Comment { get; set; }
        public ProjectStatus Status { get; set; }
        public string StatusComment { get; set; }
        public bool IsAdmin { get; set; }
    }
}