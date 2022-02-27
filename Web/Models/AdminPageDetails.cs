using Application.DTOs;
using System.Collections.Generic;

namespace Web.Models
{
    public class AdminPageDetails
    {
        public List<ProjectModel> Projects { get; set; }
        public List<EmployeeModel> Employees { get; set; }
        public List<ClientDTO> Clients { get; set; }
    }
}
