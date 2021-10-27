using System;

namespace Application.DTOs
{
    public class ProjectActivityDto
    {
        public string ProjectId { get; set; }
        public string EmployeeId { get; set; }
        public double Duration { get; set; }
        public DateTime DateTime { get; set; }
        public string Comment { get; set; }
    }
}