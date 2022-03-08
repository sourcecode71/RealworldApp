using System;

namespace Application.DTOs
{
    public class EpmProjectsDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Client { get; set; }
        public double BHours { get; set; }
        public string EmpId { get; set; }
        public string EmpName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string StartDateStr { get; set; }
        public string DeliveryDateStr { get; set; }
    }
}
