using Application.DTOs;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistance.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Core.Employees
{
    public class GetActivitiesForProject
    {
        public class Query : IRequest<List<ProjectActivityDto>>
        {
            public int SelfProjectId { get; set; }
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
                List<ProjectActivity> activities = await _context.ProjectActivities.Include(x => x.Employee).Include(x => x.Project).Where(x => x.Project.SelfProjectId == request.SelfProjectId).ToListAsync();

                List<ProjectActivityDto> activityDtos = activities.Select(x => new ProjectActivityDto
                {
                    Comment = x.Comment,
                    DateTime = x.DateTime,
                    Duration = x.Duration,
                    Status = (int)x.Status,
                    EmployeeEmail = x.EmployeeEmail,
                    StatusComment = x.StatusComment,
                    EmployeeName = x.Employee.Name
                }).ToList();

                return activityDtos;
            }
        }
    }
}