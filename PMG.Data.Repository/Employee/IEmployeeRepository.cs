using Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PMG.Data.Repository.Employee
{
    public interface IEmployeeRepository
    {
        Task<List<EmployeeDto>> GetAllActiveEmployee();
        Task<List<EpmProjectsDto>> GetEmpProjects(string empId);
    }
}
