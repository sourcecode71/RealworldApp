using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Projects
{
    public class WorkOrder : BasedModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [MaxLength(15)]
        public string WorkOrderNo { get; set; }
        public string ProjectId { get; set; }
        public virtual Project Project { get; set; }
        [MaxLength(10)]
        public string ProjectNo { get; set; }
        public double ApprovedBudget { get; set; }
        public DateTime ApprovalDate { get; set; }
        [MaxLength(250)]
        public string OTDescription { get; set; }
        [MaxLength(250)]
        public string Comments { get; set; }
        [MaxLength(200)]
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
    }

    public class HisWorkOrder : BasedModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid WorkOrderId { get; set; }
        public virtual WorkOrder WorkOrder { get; set; }
        public string WorkOrderNo { get; set; }
        public string ProjectId { get; set; }
        public virtual Project Project { get; set; }
        [MaxLength(10)]
        public string ProjectNo { get; set; }
        public double ApprovedBudget { get; set; }
        public DateTime ApprovalDate { get; set; }
        [MaxLength(250)]
        public string OTDescription { get; set; }
        [MaxLength(250)]
        public string Comments { get; set; }
        [MaxLength(200)]
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }

    }

}
