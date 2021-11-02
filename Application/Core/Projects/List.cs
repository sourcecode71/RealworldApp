using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.DTOs;
using Domain;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistance;
using Persistance.Context;

namespace Application.Core.Projects
{
    public class List
    {
        public class Query : IRequest<List<ProjectDto>>
        {
        }

        public class Handler : IRequestHandler<Query, List<ProjectDto>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }
            private string GetStatusString(ProjectStatus status)
            {
                switch (status)
                {
                    case ProjectStatus.OnTime:
                        return "On Time";
                    case ProjectStatus.Delayed:
                        return "Delayed";
                    case ProjectStatus.Modified:
                        return "Modified";
                    case ProjectStatus.Archived:
                        return "Archived";
                    default:
                        return "On Time";
                }
            }

            public async Task<List<ProjectDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var projectsDto = new List<ProjectDto>();
                var projects = await _context.Projects.Include(x => x.ProjectEmployees).Where(x => x.Status != ProjectStatus.Archived).ToListAsync();
                foreach (var item in projects)
                {
                    var engId = item.ProjectEmployees.FirstOrDefault(x => x.ProjectId == item.Id && x.EmployeeType == EmployeeType.Engineering).EmployeeId;
                    var drawId = item.ProjectEmployees.FirstOrDefault(x => x.ProjectId == item.Id && x.EmployeeType == EmployeeType.Drawing).EmployeeId;
                    var appId = item.ProjectEmployees.FirstOrDefault(x => x.ProjectId == item.Id && x.EmployeeType == EmployeeType.Approval).EmployeeId;

                    var itemDto = new ProjectDto
                    {
                        Name = item.Name,
                        Description = item.Description,
                        SelfProjectId = item.SelfProjectId,
                        Balance = item.Balance,
                        Budget = item.Budget,
                        EStatus = item.EStatus,
                        Factor = item.Factor,
                        Paid = item.Paid,
                        Progress = item.Progress,
                        Schedule = item.Schedule,
                        DeliveryDate = item.DeliveryDate,
                        Client = item.Client,
                        Engineering = _context.Employees.FirstOrDefault(x => x.Id == engId)?.Name,
                        Drawing = _context.Employees.FirstOrDefault(x => x.Id == drawId)?.Name,
                        Approval = _context.Employees.FirstOrDefault(x => x.Id == appId)?.Name,
                        Status = GetStatusString(item.Status),
                        AdminDelayedComment = item.AdminDelayedComment,
                        AdminModifiedComment = item.AdminModifiedComment
                    };
                    projectsDto.Add(itemDto);
                }
                return projectsDto;
            }
        }
    }
}