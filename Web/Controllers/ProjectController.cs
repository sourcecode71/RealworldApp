using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Models;
using Web.Services;

namespace Web.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ApiService _apiService;

        public ProjectController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public IActionResult Index(int id)
        {
            string currentEmail = HttpContext.Session.GetString("current_user_email");

            if (string.IsNullOrEmpty(currentEmail))
            {
                return RedirectToAction("Login", "Home");
            }

            ProjectPageDetails projectPage = new ProjectPageDetails();

            ProjectModel project = _apiService.CallGetProject(id).Result;
            projectPage.Project = project;
            projectPage.AllEmployees = _apiService.CallGetEmployeesNames().Result;
            projectPage.Activities = _apiService.CallGetActivitiesForProject(id).Result;

            return View(projectPage);
        }

        public IActionResult ProjectActivities()
        {
            string currentRole = HttpContext.Session.GetString("current_user_role");

            if (string.IsNullOrEmpty(currentRole))
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                if (currentRole == "Admin" || currentRole == "Management")
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Forbidden", "Home");
                }

            }
        }

        public IActionResult ProjectWorkOrder()
        {
            string currentRole = HttpContext.Session.GetString("current_user_role");

            if (string.IsNullOrEmpty(currentRole))
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                if (currentRole == "Admin" || currentRole == "Management")
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Forbidden", "Home");
                }

            }
        }

       

        public ResultModel AddActivity(ActivityModel activity)
        {
            return _apiService.CallAddActivity(activity).Result;
        }
    }
}