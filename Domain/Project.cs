using Domain.Enums;
using System;
using System.Collections.Generic;

namespace Domain
{
    public class Project
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int SelfProjectId { get; set; }
        public string Client { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int Schedule { get; set; }
        public int Progress { get; set; }
        public double Budget { get; set; }
        public double Paid { get; set; }
        public double Balance { get; set; }
        public double Factor { get; set; }
        public string EStatus { get; set; }
        public bool Invoiced { get; set; }
        public ProjectStatus Status { get; set; }
        public string AdminDelayedComment { get; set; }
        public string AdminModifiedComment { get; set; }
        public List<ProjectActivity> Activities { get; set; } = new List<ProjectActivity>();
        public virtual ICollection<ProjectEmployee> ProjectEmployees { get; set; } = new List<ProjectEmployee>();
    }
}