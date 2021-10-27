using System.Collections.Generic;

namespace Application.DTOs
{
    public class EmployeeDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public List<ProjectActivityDto> ProjectActivities { get; set; } = new List<ProjectActivityDto>();
        public List<ProjectDto> Projects { get; set; } = new List<ProjectDto>();
    }
}