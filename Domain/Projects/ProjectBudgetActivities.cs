using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Projects
{
    public class ProjectBudgetActivities : BasedModel
    {
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string ProjectId { get; set; }
        public virtual Project Project { get; set; }
        [MaxLength(10)]
        public string ProjectNo { get; set; }
        [MaxLength(12)]
        public string BudgetNo { get; set; }
        public double Budget { get; set; }
        public DateTime BudgetSubmitDate { get; set; }
        public double? ApprovedBudget { get; set; }
        public double? BalanceBudget { get; set; }
        public int Status { get; set; } = 0;
        [MaxLength(250)]
        public string Comments { get; set; }
        public DateTime  ApprovedDate { get; set; }
        public string  ApprovalSetUser { get; set; }

    }
}
