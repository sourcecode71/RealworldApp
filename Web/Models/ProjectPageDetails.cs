using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models
{
    public class ProjectPageDetails
    {
        public ProjectModel Project { get; set; }
        public List<string> AllEmployees { get; set; }
    }
}
