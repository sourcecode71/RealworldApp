using System;

namespace Web.Models
{
    public class ActivityModel
    {
        public string Id { get; set; }
        public string ProjectName { get; set; }
        public string ProjectId { get; set; }
        public int SelfProjectId { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeEmail { get; set; }
        public string EmployeeName { get; set; }
        public double Duration { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        public string Comment { get; set; }
        public int Status { get; set; }
        public string StatusComment { get; set; }
        public bool IsAdmin { get; set; }
    }
}
