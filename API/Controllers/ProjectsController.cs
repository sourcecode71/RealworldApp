using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Projects;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistance;

namespace API.Controllers
{
    public class ProjectsController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<List<Project>>> GetProjects()
        {
            return await Mediator.Send(new List.Query());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProjectById(string id)
        {
            return await Mediator.Send(new GetById.Query{Id=id});
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject(Project project)
        {
            return Ok(await Mediator.Send(new Create.Command{Project=project})); 
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(string id)
        {
            return Ok(await Mediator.Send(new Delete.Command{Id=id}));
        }
       


    }
}