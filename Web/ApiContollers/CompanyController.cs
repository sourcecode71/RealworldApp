using Application.DTOs;
using Domain.Common;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PMG.Data.Repository.Projects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.ApiControllers;

namespace Web.ApiContollers
{

    public class CompanyController : BaseApiController
    {
        private ICompanyRepository _cmRepository;

        public CompanyController(ICompanyRepository cmRepository)
        {
            _cmRepository = cmRepository;
        }

        [HttpGet("all-clients")]
        public ActionResult GetAllCientsResult()
        {
            var project =  _cmRepository.GetAllClient();
            return Ok(project);
        }

        [HttpPost("save-client")]
        public async Task<IActionResult> SaveClient(ClientDTO dTO)
        {
            bool savingStatus = await _cmRepository.SaveCleint(dTO);
            return Ok(savingStatus);
        }

        [HttpGet("all-companies")]
        public async Task<ActionResult> GetAllCompaniesResult()
        {
            var project = await _cmRepository.GetAllCompany();
            return Ok(project);
        }

        [HttpGet("all-companies-client")]
        public async Task<ActionResult> GetAllCompaniesByClient(Guid guid)
        {
            var project = await _cmRepository.GetAllCompany(guid);
            return Ok(project);
        }

        [HttpPost("save-company")]
        public async Task<IActionResult> SaveCompany(CompanyDTO dTO)
        {
            bool savingStatus = await _cmRepository.SaveCompany(dTO);
            return Ok(savingStatus);
        }


        [HttpPost("save-hour-log")]
        public async Task<IActionResult> SaveEmployeeHourlog(HourlogsDTO dTO)
        {
            string currentEmail = HttpContext.Session.GetString("current_user_email");

            if (!string.IsNullOrEmpty(currentEmail))
            {
                dTO.EmpId = new Guid(HttpContext.Session.GetString("current_user_id"));
                bool savingStatus = await _cmRepository.SaveEmployeHourLog(dTO);
                return Ok(savingStatus);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        [Authorize]
        [HttpGet("load-hour-log")]
        public async Task<IActionResult> LoadAllHourLog()
        {
            string currentEmail = HttpContext.Session.GetString("current_user_email");

            if (!string.IsNullOrEmpty(currentEmail))
            {
                string currentRole = HttpContext.Session.GetString("current_user_role");

                if (string.IsNullOrEmpty(currentRole))
                {
                    return RedirectToAction("Login", "Home");
                }

                var empId = HttpContext.Session.GetString("current_user_id");


                if (currentRole == "Admin")
                {
                    var logs = await _cmRepository.GetAllHourLogs(empId, EmployeeType.Admin);
                    return Ok(logs);
                }
                else if(currentRole == "Engineering")
                {
                    var logs = await _cmRepository.GetAllHourLogs(empId, EmployeeType.Engineering);
                    return Ok(logs);
                }
                else if (currentRole == "Drawing")
                {
                    var logs = await _cmRepository.GetAllHourLogs(empId, EmployeeType.Drawing);
                    return Ok(logs);
                }
                else
                {
                    return Ok(null);
                }
            }
            else
            {
                return Ok(null);
            }
                    

        }


    }
}
