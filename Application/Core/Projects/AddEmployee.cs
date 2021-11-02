using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistance.Context;

namespace Application.Core.Projects
{
    public class AddEmployee
    {
        public class Command : IRequest<Unit>
        {
            public int SelfProjectId { get; set; }
            public List<string> EmployeesEmails { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var project = await _context.Projects.FirstOrDefaultAsync(x => x.SelfProjectId == request.SelfProjectId);

                if (project != null)
                {
                    foreach (var employeeName in request.EmployeesEmails)
                    {
                        Employee employeeInDb = _context.Employees.FirstOrDefault(x => x.Email == employeeName);

                        if (employeeInDb != null)
                        {
                            ProjectEmployee projectEmployee = new ProjectEmployee
                            {
                                EmployeeId = employeeInDb.Id,
                                ProjectId = project.Id,
                                EmployeeType = EmployeeType.Other
                            };

                            _context.ProjectEmployees.Add(projectEmployee);
                        }
                    }

                    _context.SaveChanges();
                }


                return Unit.Value;
            }

            private Unit SystemException(string v)
            {
                throw new System.NotImplementedException(v);
            }
        }
    }
}