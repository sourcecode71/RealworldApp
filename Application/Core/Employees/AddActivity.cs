using Domain;
using Domain.Enums;
using MediatR;
using Persistance.Context;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Core.Employees
{
    public class AddActivity
    {
        public class Command : IRequest<Unit>
        {
            public string EmployeeEmail { get; set; }
            public int SelfProjectId { get; set; }
            public double Duration { get; set; }
            public string Comment { get; set; }
            public ProjectStatus Status { get; set; }
            public string StatusComment { get; set; }
            public bool IsAdmin { get; set; }
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
                var project = _context.Projects.FirstOrDefault(x => x.SelfProjectId == request.SelfProjectId);

                var emp = _context.Employees.FirstOrDefault(x => x.Email == request.EmployeeEmail);

                if (project != null && emp != null)
                {
                    var activity = new ProjectActivity
                    {
                        Id = Guid.NewGuid().ToString(),
                        Project = project,
                        SelfProjectId = request.SelfProjectId,
                        EmployeeEmail = request.EmployeeEmail,
                        Employee = emp,
                        EmployeeId = emp.Id,
                        Duration = request.Duration,
                        Comment = request.Comment,
                        DateTime = DateTime.Now,
                        Status = request.Status,
                        StatusComment = request.StatusComment,
                        IsAdmin = request.IsAdmin,
                    };

                    project.Status = request.Status;

                    _context.ProjectActivities.Add(activity);
                    project.Activities.Add(activity);

                    var result = await _context.SaveChangesAsync() > 0;
                    if (!result) return SystemException("Error");
                }

                return Unit.Value;
            }

            private Unit SystemException(string v)
            {
                throw new NotImplementedException();
            }
        }
    }
}