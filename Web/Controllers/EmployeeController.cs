using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Models;
using Web.Services;

namespace Web.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApiService _apiService;

        public EmployeeController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public IActionResult Index()
        {
            string currentEmail = HttpContext.Session.GetString("current_user_email");

            EmployeePageDetails employeePageDetails = new EmployeePageDetails();

            employeePageDetails.Projects = _apiService.CallGetProjects().Result;

            if (!string.IsNullOrEmpty(currentEmail))
            {
                employeePageDetails.Activities = _apiService.CallGetActivities(currentEmail).Result;
            }

            return View(employeePageDetails);
        }

        public List<ProjectModel> GetProjects()
        {
            return  _apiService.CallGetProjects().Result;
        }
    }
}