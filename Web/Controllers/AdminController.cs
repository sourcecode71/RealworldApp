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
            return View();
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