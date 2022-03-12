using System;
using System.Collections.Generic;

namespace Application.DTOs
{
    public class WorkOrderDTO
    {
        public WorkOrderDTO()
        {
            Id = Guid.NewGuid();
            
        }
        public Guid Id { get; set; }

        public string WorkOrderId { get; set; }
        public string ConsecutiveWork { get; set; }
        public string ProjectId { get; set; }
        public string CompanyId { get; set; }
        public string ProjectNo { get; set; }
        public int ProjectYear { get; set; }
        public double ApprovedBudget { get; set; }
        public double OriginalBudget { get; set; }
        public int Status { get; set; }
        public DateTime ApprovedDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string StartDateStr { get; set; }
        public string EndDateStr { get; set; }
        public string ApprovedDateStr { get; set; }
        public string Comments { get; set; }
        public string OTDescription { get; set; }

        public string ProjectName { get; set; }
        public string ClinetName { get; set; }
        public double ProjectBudget { get; set; }
        public string WorkOrderNo { get; set; }
        public string SetUser { get; set; }
        public List<ProjectEmp> Engineers { get; set; }
        public List<ProjectEmp> Drawings { get; set; }

    }
}
