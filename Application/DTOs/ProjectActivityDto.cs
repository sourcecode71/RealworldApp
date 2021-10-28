using System;
using Domain.Enums;

namespace Application.DTOs
{
    public class ProjectActivityDto
    {
        public int SelfProjectId { get; set; }
        public string EmployeeEmail { get; set; }
        public double Duration { get; set; }
        public DateTime DateTime { get; set; }
        public string Comment { get; set; }
        public ProjectStatus Status { get; set; }
        public string StatusComment { get; set; }
    }
}