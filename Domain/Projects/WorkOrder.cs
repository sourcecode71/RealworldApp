using Domain.Common;
using Domain.Enums;
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
        [MaxLength(150)]
        public string ConsWork { get; set; }
        public string ProjectId { get; set; }
        public virtual Project Project { get; set; }
        public Guid CompanyId { get; set; }

        [MaxLength(10)]
        public string ProjectNo { get; set; }
        public double OriginalBudget { get; set; }
        public double ApprovedBudget { get; set; }
        public int BudgetStatus { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime ApprovalDate { get; set; }
        public DateTime BudgetUpdateDate { get; set; }
        [MaxLength(250)]
        public string OTDescription { get; set; }
        [MaxLength(250)]
        public string Comments { get; set; }
        [MaxLength(200)]
        public string UpdateUser { get; set; }
        public ProjectStatus Status { get; set; } = ProjectStatus.Budgeted;
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
        public ProjectStatus Status { get; set; }

        public int ChangeFor { get; set; } = 1; // 1= Normal , 2=Budget, 3=Progress change

    }

}
