using Domain;
using System;

namespace Application.DTOs
{
    public class HourlogsDTO
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public virtual Project Projects { get; set; }
        public Guid EmpId { get; set; }
        public Guid WorkOrderId { get; set; }
        public string ProjectNo { get; set; }
        public string Project { get; set; }
        public string EmpName { get; set; }
        public string EmpType { get; set; }
        public virtual ProjectEmployee ProjectEmployees { get; set; }
        public double SpentHour { get; set; }
        public double BalanceHour { get; set; }
        public DateTime SpentDate { get; set; }
        public string SpentDateStr { get; set; }
        public string Remarks { get; set; }
    }
}
