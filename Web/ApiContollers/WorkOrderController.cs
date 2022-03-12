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
        public async Task<ActionResult> SaveWorkOrder(WorkOrderDTO dTO)
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

        [HttpPut("status-change")]
        public async Task<ActionResult> UpdateWorkOrderStatus(WorkOrderDTO dTO)
        {
            bool isSuccess = await _woRepository.UpdateWorkOrderStatus(dTO);
            return Ok(isSuccess);
        }

        [HttpGet("load-approved-orders")]
        public ActionResult GetAllApprovedOrders()
        {
            var wrkList = _woRepository.LoadAllWorkOrders();
            return Ok(wrkList);
        }

        [HttpGet("work-order-search")]
        public ActionResult GetFilteredWorkOrder(string strOT)
        {
            var wrkList = _woRepository.GetFilteredWorkOrder(strOT);
            return Ok(wrkList);
        }

        [HttpPost("save-invoic")]
        public async Task<ActionResult> SaveInvoice(InvoiceDTO invDTO)
        {
            var invStatus = await _woRepository.SaveInvoice(invDTO);
            return Ok(invStatus);
        }

    }
}
