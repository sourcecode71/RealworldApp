using Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Projects
{
    public class WorkOrderEmployee {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid WorkOrderId { get; set; }
        public virtual WorkOrder WorkOrder { get; set; }
        public double BudgetHours { get; set; }
        public double TotalHourLog { get; set; }
        public string EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }
        public EmployeeType EmployeeType { get; set; }

    }




}
