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
                    case ProjectStatus.Invoiced:
                        return "Invoiced";
                    default:
                        return "On Time";
                }
            }

            public async Task<ProjectDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var project = await _context.Projects.Include(x => x.ProjectEmployees).Include(a => a.Activities).FirstOrDefaultAsync(x => x.SelfProjectId == request.SelfProjectId);

                List<string> projectEmployees = project.ProjectEmployees.Select(x => x.EmployeeId).ToList().Distinct().ToList();

                List<Employee> employees = new List<Employee>();

                foreach (var employee in projectEmployees)
                {
                    employees.Add(_context.Employees.FirstOrDefault(x => x.Id == employee));
                }

                string employeesNames = string.Empty;

                foreach (var employee in employees)
                {
                    employeesNames += employee.Name + ", ";
                }

                var eng = project.ProjectEmployees.FirstOrDefault(x => x.EmployeeType == EmployeeType.Engineering);
                string engName = string.Empty;

                if (eng != null)
                {
                    var employee = _context.Employees.FirstOrDefault(x => x.Id == eng.EmployeeId);

                    if (employee != null)
                        engName = employee.Name;
                }

                var draw = project.ProjectEmployees.FirstOrDefault(x => x.EmployeeType == EmployeeType.Drawing);
                string drawName = string.Empty;

                if (draw != null)
                {
                    var employee = _context.Employees.FirstOrDefault(x => x.Id == draw.EmployeeId);

                    if (employee != null)
                        drawName = employee.Name;
                }

                var app = project.ProjectEmployees.FirstOrDefault(x =>x.EmployeeType == EmployeeType.Approval);
                string appName = string.Empty;

                if (app != null)
                {
                    var employee = _context.Employees.FirstOrDefault(x => x.Id == app.EmployeeId);

                    if (employee != null)
                        appName = employee.Name;
                }

                var lastModifiedActivity = project.Activities.OrderBy(x => x.DateTime).LastOrDefault(x => x.Status == ProjectStatus.Modified);
                var lastDelayedActivity = project.Activities.OrderBy(x => x.DateTime).LastOrDefault(x => x.Status == ProjectStatus.Delayed);

                var projectDto = new ProjectDto
                {
                    Name = project.Name,
                    Description = project.Description,
                    SelfProjectId = project.SelfProjectId,
                    Balance = project.Balance,
                    Budget = project.Budget,
                    EStatus = project.EStatus,
                    Factor = project.Factor,
                    Paid = project.Paid,
                    Progress = Math.Abs(Math.Round((DateTime.Now - project.CreatedDate).TotalDays / 7, 2)),
                    Schedule = project.Schedule,
                    DeliveryDate = project.DeliveryDate,
                    Client = project.Client,
                    Engineering = engName,
                    Drawing = drawName,
                    Approval =appName,
                    Status = GetStatusString(project.Status),
                    AdminDelayedComment = project.AdminDelayedComment,
                    AdminModifiedComment = project.AdminModifiedComment,
                    EmployeeDelayedComment = lastDelayedActivity != null ? lastDelayedActivity.StatusComment : string.Empty,
                    EmployeeModifiedComment = lastModifiedActivity != null ? lastModifiedActivity.StatusComment : string.Empty,
                    EmployeesNames = employeesNames,
                    Activities = project.Activities.Select(x => new ProjectActivityDto
                    {
                        SelfProjectId = x.SelfProjectId,
                        EmployeeEmail = x.EmployeeEmail,
                        Duration = x.Duration,
                        DateTime = x.DateTime,
                        Comment = x.Comment,
                        Status = (int)x.Status,
                        StatusComment = x.StatusComment
                    }).ToList()
                };

                return projectDto;
            }
        }
    }
}