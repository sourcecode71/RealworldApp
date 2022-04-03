using Application.DTOs;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PMG.Data.Repository.Projects;
using System;
using System.Linq;
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
            string currentEmail = HttpContext.Session.GetString("current_user_email");

            if (!string.IsNullOrEmpty(currentEmail))
            {
                bool isSuccess = await _woRepository.UpdateWorkOrderStatus(dTO);
                return Ok(isSuccess);
            }
            else
            {
                return RedirectToAction("Forbidden", "Home");
            }
        }

        [HttpGet("load-approved-orders/emp-wise")]
        public async Task<IActionResult> GetAllApprovedOrdersByEmp()
        {
            try
            {
                string currentEmail = HttpContext.Session.GetString("current_user_email");

                if (!string.IsNullOrEmpty(currentEmail))
                {
                    var empId = HttpContext.Session.GetString("current_user_id");
                    var wrkList = await _woRepository.LoadAllWorkOrdersByEmp(empId);
                    return Ok(wrkList);
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
                
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


        [HttpGet("load-approved-orders")]
        public ActionResult GetAllWorkOrder()
        {
            var wrkList = _woRepository.LoadAllWorkOrders();
            return Ok(wrkList);
        }


        [HttpGet("load-work-orders/archived")]
        public ActionResult GetAllArchivedWorkOrder()
        {
            var wrkList = _woRepository.LoadAllWorkOrders();
            var filterWrk = wrkList.Where(w=>w.WrkStatus == ProjectStatus.Archived || w.WrkStatus == ProjectStatus.Completed);
            return Ok(filterWrk);
        }

        [HttpGet("load-work-orders/active")]
        public ActionResult GetAllActiveWorkOrder()
        {
            try
            {
                var wrkList = _woRepository.LoadAllWorkOrders();
                var filterWrk = wrkList.Where(w => w.WrkStatus != ProjectStatus.Archived || w.WrkStatus != ProjectStatus.Completed);
                return Ok(filterWrk);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet("work-orders/by-project")]
        public async Task<ActionResult> GetWorkOrderByProject(string PrId)
        {
            var wrkList = await _woRepository.WorkOrderByProjects(PrId);
            return Ok(wrkList);
        }


        [HttpGet("work-orders-by-id")]
        public async Task<ActionResult> GetWorkOrderByWorkOrderId(string WrkId)
        {
            var wrkList = await _woRepository.LoadWorkOrdersById(WrkId);
            return Ok(wrkList);
        }




    }
}
