using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.DTOs;
using Domain;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistance.Context;

namespace Application.Core.Employees
{
    public class ListEmployees
    {
        public class Query : IRequest<List<EmployeeDto>>
        {
        }

        public class Handler : IRequestHandler<Query, List<EmployeeDto>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<List<EmployeeDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var employeesDto = new List<EmployeeDto>();
                var employees = await _context.Employees.Include(x => x.ProjectEmployees).ToListAsync();
                foreach (var item in employees)
                {
                    List<string> projects = item.ProjectEmployees.Select(x => x.ProjectId).ToList().Distinct().ToList();

                    List<Project> projectsInDb = new List<Project>();

                    foreach(var project in projects)
                    {
                        var projectInDb = _context.Projects.FirstOrDefault(x => x.Id == project );

                        if(projectInDb != null && projectInDb.Status != ProjectStatus.Archived)
                            projectsInDb.Add(projectInDb);
                    }

                    List<string> projectsNames = new List<string>();

                    if (projectsInDb.Count > 0)
                    {
                        foreach (var project in projectsInDb)
                        {
                            projectsNames.Add(project.Name + ", ");
                        }
                    }

                    var itemDto = new EmployeeDto
                    {
                        Name = item.Name,
                        Email = item.Email,
                        ProjectsNames = projectsInDb.Count > 0 ? projectsInDb.Select(x => x.Name).ToList() : new List<string>()
                    };
                    employeesDto.Add(itemDto);
                }
                return employeesDto;
            }
        }
    }
}