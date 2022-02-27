using Application.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
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
                    AdminPageDetails adminPage = new AdminPageDetails();

                    List<ProjectModel> projects = _apiService.CallGetProjects().Result;
                    List<EmployeeModel> employees = _apiService.CallGetEmployees().Result;
                    List<ClientDTO> clientModels = _apiService.CallAllClient().Result;


                    adminPage.Projects = projects.Where(x => x.Status != "Archived").ToList();
                    adminPage.Employees = employees;
                    adminPage.Clients = clientModels;

                    return View(adminPage);
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            }

            return RedirectToAction("Login", "Home");
        }

        public IActionResult AddEmployee()
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
                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            }

            return RedirectToAction("Login", "Home");
        }

        public IActionResult AddProject()
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
                    AdminPageDetails adminPage = new AdminPageDetails();

                    List<EmployeeModel> employees = _apiService.CallGetEmployees().Result;
                    List<ClientDTO> clientModels = _apiService.CallAllClient().Result;

                    adminPage.Employees = employees;
                    adminPage.Clients = clientModels;

                    return View(adminPage);
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            }

            return RedirectToAction("Login", "Home");
        }

        public IActionResult GenerateReports()
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
                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            }

            return RedirectToAction("Login", "Home");
        }

        public IActionResult Archived()
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
                    AdminPageDetails adminPage = new AdminPageDetails();

                    List<ProjectModel> projects = _apiService.CallGetProjects().Result;
                    adminPage.Projects = projects.Where(x => x.Status == "Completed" || x.Status == "Invoiced" || x.Status == "Archived").ToList();

                    return View(adminPage);
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

        public ResultModel AddNewProject(ProjectModel project)
        {
            return _apiService.CallCreateProject(project).Result;
        }

        public List<string> GetEmployeesNames()
        {
            return  _apiService.CallGetEmployeesNames().Result;
        }

        public ResultModel ArchiveProject(ProjectModel project)
        {
            return _apiService.CallArchiveProject(project).Result;
        }

        public ResultModel InvoiceProject(ProjectModel project)
        {
            return _apiService.CallInvoiceProject(project).Result;
        }

        public ResultModel AssignEmployee(ProjectModel project)
        {
            return _apiService.CallAssignEmployee(project).Result;
        }

        public ResultModel SaveDelayedComment(ProjectModel project)
        {
            return _apiService.CallSaveDelayedComment(project).Result;
        }

        public ResultModel SaveModifiedComment(ProjectModel project)
        {
            return _apiService.SaveModifiedComment(project).Result;
        }

        public ResultModel CompleteProject(CompleteProjectModel project)
        {
            return _apiService.CallCompleteProject(project).Result;
        }

        public ResultModel SavePaid(ProjectModel project)
        {
            return _apiService.CallSavePaid(project).Result;
        }
    }
}