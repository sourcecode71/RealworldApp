using System.Collections.Generic;

namespace Application.DTOs
{
    public class EmployeeDto
    {
        public EmployeeDto()
        {
            this.Name = string.Format("{0} {1}", this.FirstName, this.LastName);
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
        public string NewPassword { get; set; }
        public string CurrentPassword { get; set; }
        public List<string> ProjectsNames { get; set; }

        public List<ProjectActivityDto> ProjectActivities { get; set; } = new List<ProjectActivityDto>();
        public List<ProjectDto> Projects { get; set; } = new List<ProjectDto>();
    }
}