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

                string engId = project.ProjectEmployees.FirstOrDefault(x => x.EmployeeType == EmployeeType.Engineering).EmployeeId;
                string drawId = project.ProjectEmployees.FirstOrDefault(x => x.EmployeeType == EmployeeType.Drawing).EmployeeId;
                string appId = project.ProjectEmployees.FirstOrDefault(x => x.EmployeeType == EmployeeType.Approval).EmployeeId;

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
                    Progress = Math.Round((DateTime.Now - project.DeliveryDate).TotalDays / 7, 2),
                    Schedule = project.Schedule,
                    DeliveryDate = project.DeliveryDate,
                    Client = project.Client,
                    Engineering = _context.Employees.FirstOrDefault(x => x.Id == engId).Name,
                    Drawing = _context.Employees.FirstOrDefault(x => x.Id == drawId).Name,
                    Approval = _context.Employees.FirstOrDefault(x => x.Id == appId).Name,
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