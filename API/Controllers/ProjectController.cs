using Application.Core.Projects;
using Domain;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class ProjectController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<List<Project>>> GetProjects()
        {
            return await Mediator.Send(new List.Query());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProjectById(string id)
        {
            return await Mediator.Send(new GetById.Query { Id = id });
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromForm] Project project, [FromForm] List<string> employees)
        {
            return Ok(await Mediator.Send(new Create.Command { Project = project, Employees=employees }));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(string id)
        {
            return Ok(await Mediator.Send(new Delete.Command { Id = id }));
        }
    }
}