using System;

namespace Application.DTOs
{
    public class WorkOrderDTO
    {
        public Guid Id { get; set; }

        public Guid WorkOrderId { get; set; }
        public string ProjectId { get; set; }
        public string ProjectNo { get; set; }
        public int ProjectYear { get; set; }
        public string  OTDescription { get; set; }
        public double ApprovedBudget { get; set; }
        public DateTime ApprovedDate { get; set; }
        public string ApprovedDateStr { get; set; }
        public string Comments { get; set; }

        public string ProjectName { get; set; }
        public string ClinetName { get; set; }
        public double ProjectBudget { get; set; }
        public string WorkOrderNo { get; set; }

    }
}
