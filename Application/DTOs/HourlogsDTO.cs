using Domain;
using System;

namespace Application.DTOs
{
    public class HourlogsDTO
    {
        public Guid Id { get; set; }
        public string ProjectId { get; set; }
        public Guid EmpId { get; set; }
        public Guid WorkOrderId { get; set; }
        public string WorkOrderNo { get; set; }
        public string WorkOrderName { get; set; }
        public string ProjectNo { get; set; }
        public string ProjectName { get; set; }
        public int Year { get; set; }
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
