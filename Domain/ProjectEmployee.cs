using Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class ProjectEmployee
    {
        public int Id { get; set; }
        public string ProjectId { get; set; }
        public string EmployeeId { get; set; }

        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }
        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }

        public EmployeeType EmployeeType { get; set; }
    }
}