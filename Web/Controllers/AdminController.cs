using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Web.Models;
using Web.Services;

namespace Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApiService _apiService;

        public AdminController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public IActionResult Index()
        {
            string currentEmail = HttpContext.Session.GetString("current_user_email");

            if (!string.IsNullOrEmpty(currentEmail))
            {
                string currentRole = HttpContext.Session.GetString("current_user_role");

                if (string.IsNullOrEmpty(currentRole))
                {
                    return RedirectToAction("Login", "Home");
                }

                if (currentRole == "Admin")
                {
                    List<ProjectModel> projects = _apiService.CallGetProjects().Result;

                    return View(projects);
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            }

            return RedirectToAction("Login", "Home");
        }

        public ResultModel AddNewEmployee(EmployeeModel employee)
        {
            return _apiService.CallRegisterUser(employee).Result;
        }

        public List<string> GetEmployeesNames()
        {
            return  _apiService.CallGetEmployeesNames().Result;
        }
    }
}