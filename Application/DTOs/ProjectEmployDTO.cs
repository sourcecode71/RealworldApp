using Domain.Enums;

namespace Application.DTOs
{
    public class ProjectEmployDTO
    {
        public EmployeeType EmpType { get; set; }
        public string EmpId { get; set; }
        public double BudgetHours { get; set; }
    }
}
