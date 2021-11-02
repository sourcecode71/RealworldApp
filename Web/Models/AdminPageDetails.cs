using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models
{
    public class AdminPageDetails
    {
        public List<ProjectModel> Projects { get; set; }
        public List<EmployeeModel> Employees { get; set; }
    }
}
