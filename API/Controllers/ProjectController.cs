using Application.DTOs;
using Application.Core.Projects;
using Domain;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Domain.Enums;
using Application.Core.Employees;

namespace API.Controllers
{
    public class ProjectController : BaseApiController
    {
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<ProjectDto>>> GetProjects()
        {
            return Ok(await Mediator.Send(new List.Query()));
        }

        [HttpGet("byId")]
        public async Task<ActionResult<ProjectDto>> GetProjectById(int selfProjectId)
        {
            return await Mediator.Send(new GetById.Query { SelfProjectId = selfProjectId });
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject(ProjectDto project)
        {
            Dictionary<EmployeeType, string> employees = new Dictionary<EmployeeType, string>();

            employees.Add(EmployeeType.Engineering, project.Engineering);
            employees.Add(EmployeeType.Drawing, project.Drawing);
            employees.Add(EmployeeType.Approval, project.Approval);

            Project projectDomain = new Project
            {
                Balance = project.Budget,
                Factor = project.Budget / (double)project.Schedule,
                Client = project.Client,
                DeliveryDate = project.DeliveryDate,
                Paid = 0,
                Name = project.Name,
                EStatus = project.EStatus,
                Progress = 0,
                Schedule = project.Schedule,
                Status = ProjectStatus.OnTime,
                Budget = project.Budget
            };

            return Ok(await Mediator.Send(new Create.Command { Project = projectDomain, Employees = employees }));
        }

        [HttpPost("archive")]
        public async Task<IActionResult> ArchiveProject(ProjectDto project)
        {
            return Ok(await Mediator.Send(new Archive.Command { SelfProjectId = project.SelfProjectId }));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(string id)
        {
            return Ok(await Mediator.Send(new Delete.Command { Id = id }));
        }
        [HttpPut("delayed")]
        public async Task<IActionResult> AddDelayedComment(string delayed, int selfProjectId)
        {
            return Ok(await Mediator.Send(new AddDelayedComment.Command { Delayed = delayed, SelfProjectId = selfProjectId }));
        }

        [HttpPut("modified")]
        public async Task<IActionResult> AddModifiedComment(string modified, int selfProjectId)
        {
            return Ok(await Mediator.Send(new AddModifiedComment.Command { Modified = modified, SelfProjectId = selfProjectId }));
        }

        [HttpPost("assign")]
        public async Task<IActionResult> AssignEmployee(ProjectDto details)
        {
            string[] names = details.EmployeesNames.Split(',');
            List<string> employeesNames = new List<string>();

            foreach(var name in names)
            {
                employeesNames.Add(name.Replace(" ", string.Empty));
            }

            return Ok(await Mediator.Send(new AddEmployee.Command { EmployeesEmails = employeesNames, SelfProjectId = details.SelfProjectId }));
        }

        [HttpPost("savedelayed")]
        public async Task<IActionResult> SaveDelayedComment(ProjectDto details)
        {
            return Ok(await Mediator.Send(new AddDelayedComment.Command { Delayed = details.AdminDelayedComment, SelfProjectId = details.SelfProjectId }));
        }

        [HttpPost("savemodified")]
        public async Task<IActionResult> SaveModifiedComment(ProjectDto details)
        {
            return Ok(await Mediator.Send(new AddModifiedComment.Command { Modified = details.AdminModifiedComment, Budget = details.Budget, SelfProjectId = details.SelfProjectId }));
        }


        [HttpPost("activity")]
        public async Task<ActionResult> AddActivity(ProjectActivityDto projectActivity)
        {
            return Ok(await Mediator.Send(new AddActivity.Command
            {
                EmployeeEmail = projectActivity.EmployeeEmail,
                SelfProjectId = projectActivity.SelfProjectId,
                Duration = projectActivity.Duration,
                Comment = projectActivity.Comment,
                Status = (ProjectStatus)projectActivity.Status,
                StatusComment = projectActivity.StatusComment
            }));
        }

    }
}