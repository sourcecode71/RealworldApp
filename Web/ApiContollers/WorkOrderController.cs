using Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using PMG.Data.Repository.Projects;
using System;
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
        public async Task<IActionResult> GetAllApprovedOrders()
        {
            try
            {
                var empId = "752cbd18-f618-478c-972e-65f33414dbb5";

                var wrkList = await _woRepository.LoadAllWorkOrdersByEmp(empId);
                return Ok(wrkList);
            }
            catch (Exception ex)
            {

                throw ex;
            }
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

        [HttpPost("save-invoic")]
        public async Task<ActionResult> LoadEmpWiseWorkOrder(InvoiceDTO invDTO)
        {
            var invStatus = await _woRepository.SaveInvoice(invDTO);
            return Ok(invStatus);
        }

    }
}
