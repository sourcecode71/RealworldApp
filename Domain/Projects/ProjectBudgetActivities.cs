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
        public decimal? ApprovedBudget { get; set; }
        public decimal? BalanceBudget { get; set; }
        public bool ApprovalStatus { get; set; }
        [MaxLength(250)]
        public string Comments { get; set; }
        public DateTime  ApprovedDate { get; set; }

    }
}
