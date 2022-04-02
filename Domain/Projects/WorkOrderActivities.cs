using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Projects
{
    public class WorkOrderActivities : BasedModel
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid WorkOrderId { get; set; }
        public virtual WorkOrder WorkOrder { get; set; }
        [MaxLength(13)]
        public string WorkOrderNo { get; set; }
        [MaxLength(15)]
        public string BudgetNo { get; set; }
        public string BudgetVersionNo { get; set; }
        public double Budget { get; set; }
        public DateTime BudgetSubmitDate { get; set; }
        public double? ApprovedBudget { get; set; }
        public double? BalanceBudget { get; set; }
        public int Status { get; set; } = 0;
        [MaxLength(250)]
        public string Comments { get; set; }
        public DateTime ApprovedDate { get; set; }
        [MaxLength(150)]
        public string ApprovalSetUser { get; set; }
    }
}
