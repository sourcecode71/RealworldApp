using System.Collections.Generic;

namespace Application.DTOs
{
    public class ProjectDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int SelfProjectId { get; set; }
        public string AdminDelayedComment { get; set; }
        public string AdminModifiedComment { get; set; }
        public List<ProjectActivityDto> Activities { get; set; } = new List<ProjectActivityDto>();
        public List<EmployeeDto> Employees { get; set; }
    }
}