using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Projects
{
    public class HisBudgetActivities :BasedModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string ProjectId { get; set; }
        public virtual Project Project { get;set;}
        public Guid WorkOrderId { get; set; }
        public virtual WorkOrder WorkOrder { get; set; }
        public int BudgetFor { get; set; } // 1= Work Order 2= Project 
        public string BudgetNo { get; set; }
        public double OriginalBudget { get; set; }
        public double ChangedBudget { get; set; }
        public DateTime BudgetSubmitDate { get; set; }
        public int Status { get; set; } = 0;
        [MaxLength(250)]
        public string Comments { get; set; }
        public string OriginalSetUser { get; set; }
        public DateTime OriginalSetDate { get; set; }
    }
}
