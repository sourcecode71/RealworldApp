using Application.DTOs;
using Microsoft.EntityFrameworkCore;
using Persistance.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMG.Data.Repository.Employee
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DataContext _context;
        public EmployeeRepository(DataContext dataContext)
        {
            _context = dataContext;
        }
        public async Task<List<EmployeeDto>> GetAllActiveEmployee()
        {
            try
            {
                var empList = await (from emp in _context.Employees
                                     join ur in _context.UserRoles on emp.Id equals ur.UserId
                                     join rl in _context.Roles on ur.RoleId equals rl.Id
                                     select new EmployeeDto
                                     {
                                         Id = emp.Id,
                                         Email = emp.Email,
                                         FirstName = emp.FirstName,
                                         LastName = emp.LastName,
                                         Name = string.Format("{0} {1}", emp.FirstName, emp.LastName),
                                         PhoneNumber = emp.PhoneNumber,
                                         Role = rl.Name
                                     }).ToListAsync();
                return empList;

            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<EpmProjectsDto>> GetEmpProjects(string empId)
        {
            try
            {
                var pemp = await (from ep in _context.ProjectEmployees
                                  join em in _context.Projects on ep.ProjectId equals em.Id
                                  join cn in _context.Clients on em.ClientId equals cn.Id
                                  where ep.EmployeeId == empId
                                  select (new EpmProjectsDto
                                  {
                                      Id = em.Id,
                                      BHours = ep.BudgetHours,
                                      Client = cn.Name,
                                      Name = em.Name,
                                      StartDateStr = em.StartDate.ToString("MM/dd/yyyy"),
                                      DeliveryDateStr = em.DeliveryDate.ToString("MM/dd/yyyy")
                                  })).ToListAsync();
                return pemp;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        }
    }
}
