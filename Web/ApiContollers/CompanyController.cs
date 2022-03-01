﻿using Application.DTOs;
using Domain.Common;
using Microsoft.AspNetCore.Mvc;
using PMG.Data.Repository.Projects;
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

        [HttpPost("save-company")]
        public async Task<IActionResult> SaveCompany(CompanyDTO dTO)
        {
            bool savingStatus = await _cmRepository.SaveCompany(dTO);
            return Ok(savingStatus);
        }


        [HttpPost("save-hour-log")]
        public async Task<IActionResult> SaveEmployeeHourlog(HourlogsDTO dTO)
        {
            dTO.EmpId = new System.Guid("89eac876-cd03-4049-974c-c0c759063e75");

            bool savingStatus = await _cmRepository.SaveEmployeHourLog(dTO);
            return Ok(savingStatus);
        }

    }
}