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
    public class ListByProjectStatus
    {
        public class Query : IRequest<List<ProjectDto>>
        {
            public int ProjectStatus { get; set; }
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
                    case ProjectStatus.Completed:
                        return "Completed";
                    default:
                        return "On Time";
                }
            }

            public async Task<List<ProjectDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var projectsDto = new List<ProjectDto>();
                    
                var projects = request.ProjectStatus == 0 ? await _context.Projects.Include(x => x.ProjectEmployees).ToListAsync()
                        : await _context.Projects.Include(x => x.ProjectEmployees).Where(x => x.Status == (ProjectStatus)request.ProjectStatus).ToListAsync();
                
                foreach (var item in projects)
                {
                    var eng = item.ProjectEmployees.FirstOrDefault(x => x.ProjectId == item.Id && x.EmployeeType == EmployeeType.Engineering);
                    string engName = string.Empty;

                    if (eng != null)
                    {
                        var employee = _context.Employees.FirstOrDefault(x => x.Id == eng.EmployeeId);

                        if (employee != null)
                            engName = employee.Name;
                    }

                    var draw = item.ProjectEmployees.FirstOrDefault(x => x.ProjectId == item.Id && x.EmployeeType == EmployeeType.Drawing);
                    string drawName = string.Empty;

                    if (draw != null)
                    {
                        var employee = _context.Employees.FirstOrDefault(x => x.Id == draw.EmployeeId);

                        if (employee != null)
                            drawName = employee.Name;
                    }

                    var app = item.ProjectEmployees.FirstOrDefault(x => x.ProjectId == item.Id && x.EmployeeType == EmployeeType.Approval);
                    string appName = string.Empty;

                    if (app != null)
                    {
                        var employee = _context.Employees.FirstOrDefault(x => x.Id == app.EmployeeId);

                        if (employee != null)
                            appName = employee.Name;
                    }


                    var itemDto = new ProjectDto
                    {
                        Name = item.Name,
                        Description = item.Description,
                        SelfProjectId = item.SelfProjectId,
                        Balance = item.Balance,
                        Budget = item.Budget,
                        EStatus = item.EStatus,
                        Factor =   item.Factor,
                        Paid = item.Paid,
                        Progress = Math.Abs(Math.Round((DateTime.Now - item.CreatedDate).TotalDays /7, 2)),
                        Schedule = item.Schedule,
                        DeliveryDate = item.DeliveryDate,
                        Client = item.Client,
                        Engineering = engName,
                        Drawing = drawName,
                        Approval = appName,
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