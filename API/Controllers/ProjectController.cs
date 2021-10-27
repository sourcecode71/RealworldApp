using Application.Core.Employees;
using Application.DTOs;
using Application.Core.Projects;
using Domain;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class ProjectController : BaseApiController
    {
        [HttpGet]
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
        public async Task<IActionResult> CreateProject([FromForm] Project project, [FromForm] List<string> employees)
        {
            return Ok(await Mediator.Send(new Create.Command { Project = project, Employees = employees }));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(string id)
        {
            return Ok(await Mediator.Send(new Delete.Command { Id = id }));
        }
    }
}