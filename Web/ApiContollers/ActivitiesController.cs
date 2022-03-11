using Microsoft.AspNetCore.Mvc;
using PMG.Data.Repository.Projects;

namespace Web.ApiContollers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivitiesController : ControllerBase
    {
        private readonly IProjectsRepository _project;
        public ActivitiesController(IProjectsRepository projects)
        {
            _project = projects;
        }


    }
}
