using Application.DTOs;
using Domain.Common;
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
            dTO.EmpId = new System.Guid("752cbd18-f618-478c-972e-65f33414dbb5");

            bool savingStatus = await _cmRepository.SaveEmployeHourLog(dTO);
            return Ok(savingStatus);
        }

        [HttpGet("load-hour-log")]
        public async Task<List<HourlogsDTO>> LoadAllHourLog()
        {
            var empId = "752cbd18-f618-478c-972e-65f33414dbb5";
            string type = "04";
            var logs = await _cmRepository.GetAllHourLogs(empId,type);
            return logs;

        }


    }
}
