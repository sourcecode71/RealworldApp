using System;

namespace Web.Models
{
    public class ProjectModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int SelfProjectId { get; set; }
        public string Client { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int Schedule { get; set; }
        public int Progress { get; set; }
        public double Budget { get; set; }
        public double Paid { get; set; }
        public double Balance { get; set; }
        public double Factor { get; set; }
        public string EStatus { get; set; }
        public string Status { get; set; }
        public string Engineering { get; set; }
        public string Drawing { get; set; }
        public string Approval { get; set; }
        public string EmployeesNames { get; set; }
    }
}