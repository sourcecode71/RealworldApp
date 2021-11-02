using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.DTOs;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistance.Context;

namespace Application.Core.Employees
{
    public class GetActivitiesForEmployee
    {
        public class Query : IRequest<List<ProjectActivityDto>>
        {
            public string Email { get; set; }
        }

        public class Handler : IRequestHandler<Query, List<ProjectActivityDto>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<List<ProjectActivityDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                List<ProjectActivity> activities = await _context.ProjectActivities.Include(x => x.Employee).Include(x => x.Project).Where(x => x.Employee.Email == request.Email).ToListAsync();

                List<ProjectActivityDto> activityDtos = activities.Select(x => new ProjectActivityDto
                {
                    Comment = x.Comment,
                    DateTime = x.DateTime,
                    Duration = x.Duration,
                    SelfProjectId = x.SelfProjectId,
                    Status = (int)x.Status,
                    EmployeeEmail = x.EmployeeEmail,
                    StatusComment = x.StatusComment,
                    ProjectName = x.Project.Name
                }).ToList();

                return activityDtos;
            }
        }
    }
}