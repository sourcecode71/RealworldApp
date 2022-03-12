using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Projects
{
    public class Hourlogs : BasedModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public virtual Project Projects { get; set; }
        public Guid WorkOrderId { get; set; }
        public virtual WorkOrder WorkOrder { get; set; }
        public Guid EmpId { get; set; }
        public virtual ProjectEmployee ProjectEmployees { get; set; }
        public double SpentHour { get; set; }
        public double BalanceHour { get; set; }
        public DateTime SpentDate { get; set; }
        public int HourLogFor { get; set; }   // 1=work order , 2=project
        public string Remarks { get; set; }

    }
}
