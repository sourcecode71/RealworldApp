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
        public virtual ProjectEmployee ProjectEmployees { get; set; }
        public double SpentHour { get; set; }
        public double BalanceHour { get; set; }
        public DateTime SpentDate { get; set; }
        public string Remarks { get; set; }
    }
}
