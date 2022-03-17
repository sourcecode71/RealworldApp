using System;

namespace Application.DTOs
{
    public class HourslogDto
    {
        public string EmpId { get; set; }
        public string WrkId { get; set; }
        public string ProjectNo { get; set; }
        public string WrkNo { get; set; }
        public string WrkName { get; set; }
        public string EmpName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public  string EmpType { get; set; }
        public string Role { get; set; }
        public double Bhour { get; set; }
        public double Lhour { get; set; }
        public DateTime LogDate { get; set; }
        public string LogDateStr { get; set; }
        public string Remarks { get; set; }
    }
}
