using System.Collections.Generic;

namespace Web.Models
{
    public class EmployeePageDetails
    {
        public List<ProjectModel> Projects { get; set; }
        public List<ActivityModel> Activities { get; set; } = new List<ActivityModel>();
    }
}
