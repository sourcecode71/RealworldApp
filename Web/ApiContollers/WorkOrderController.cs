using Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using PMG.Data.Repository.Projects;
using System.Threading.Tasks;

namespace Web.ApiContollers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkOrderController : ControllerBase
    {
        private readonly IProjectsRepository _project;
        private readonly IWorkOrderRepository _woRepository;
        public WorkOrderController(IProjectsRepository projects,IWorkOrderRepository orderRepository)
        {
            _project = projects;
            _woRepository = orderRepository;
        }

        [HttpPost("store-work-order")]
        public async Task<ActionResult> SaveProjectApproval(WorkOrderDTO dTO)
        {
            bool isSuccess = await _woRepository.SaveWorkOrder(dTO);
            return Ok(isSuccess);
        }

        [HttpPut("update-work-order")]
        public async Task<ActionResult> UpdateProjectApproval(WorkOrderDTO dTO)
        {
            bool isSuccess = await _woRepository.UpdateWorkOrder(dTO);
            return Ok(isSuccess);
        }

        [HttpGet("load-approved-orders")]
        public ActionResult GetAllApprovedOrders()
        {
            var wrkList = _woRepository.LoadAllWorkOrders();
            return Ok(wrkList);
        }

    }
}
