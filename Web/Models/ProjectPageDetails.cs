using System.Collections.Generic;

namespace Web.Models
{
    public class ProjectPageDetails
    {
        public ProjectModel Project { get; set; }
        public List<ActivityModel> Activities { get; set; }
        public List<string> AllEmployees { get; set; }
    }
}
