using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistance.Context;

namespace Application.Core.Employees
{
    public class GetEmployeeByEmail
    {
        public class Query : IRequest<EmployeeDto>
        {
            public string Email { get; set; }
        }

        public class Handler : IRequestHandler<Query, EmployeeDto>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<EmployeeDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var employee = await _context.Employees.Include(p => p.Projects).Include(a => a.ProjectActivities).FirstOrDefaultAsync(x => x.Email == request.Email);
                var employeeDto = new EmployeeDto
                {
                    Name = employee.Name,
                    Email = employee.Email,
                    Projects = employee.Projects.Select(x => new ProjectDto
                    {
                        Name = x.Name,
                        Description = x.Description,
                        SelfProjectId = x.SelfProjectId
                    }).ToList(),
                    ProjectActivities = employee.ProjectActivities.Select(x => new ProjectActivityDto
                    {
                        ProjectId = x.ProjectId,
                        EmployeeId = x.EmployeeId,
                        Duration = x.Duration,
                        DateTime = x.DateTime,
                        Comment = x.Comment
                    }).ToList()
                };

                return employeeDto;
            }
        }
    }
}