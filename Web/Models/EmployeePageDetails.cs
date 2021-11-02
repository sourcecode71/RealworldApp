using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models
{
    public class EmployeePageDetails
    {
        public List<ProjectModel> Projects { get; set; }
        public List<ActivityModel> Activities { get; set; } = new List<ActivityModel>();
    }
}
