using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.DTOs;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistance;
using Persistance.Context;

namespace Application.Core.Projects
{
    public class GetById
    {
        public class Query : IRequest<ProjectDto>
        {
            public int SelfProjectId { get; set; }
        }

        public class Handler : IRequestHandler<Query, ProjectDto>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<ProjectDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var project = await _context.Projects.Include(e => e.Employees).Include(a => a.Activities).FirstOrDefaultAsync(x => x.SelfProjectId == request.SelfProjectId);
                var projectDto = new ProjectDto
                {
                    Name = project.Name,
                    Description = project.Description,
                    SelfProjectId = project.SelfProjectId,
                    AdminDelayedComment = project.AdminDelayedComment,
                    AdminModifiedComment = project.AdminModifiedComment,

                    Employees = project.Employees.Select(x => new EmployeeDto
                    {
                        Name = x.Name,
                        Email = x.Email
                    }).ToList(),

                    Activities = project.Activities.Select(x => new ProjectActivityDto
                    {
                        SelfProjectId = x.SelfProjectId,
                        EmployeeEmail = x.EmployeeEmail,
                        Duration = x.Duration,
                        DateTime = x.DateTime,
                        Comment = x.Comment,
                        Status = x.Status,
                        StatusComment = x.StatusComment
                    }).ToList()
                };

                return projectDto;
            }
        }
    }
}