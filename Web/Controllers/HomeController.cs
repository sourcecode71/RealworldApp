using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Web.Models;
using Web.Services;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApiService _apiService;
        private readonly ConverterService<string> _converterService;


        public HomeController(ILogger<HomeController> logger, ApiService apiService, ConverterService<string> converterService)
        {
            _logger = logger;
            _apiService = apiService;
            _converterService = converterService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public ActionResult<ResultModel> Login(EmployeeModel loginUser)
        {
            ResultModel result = _apiService.CallLogin(loginUser).Result;

            if(result.IsSuccess)
            {
                EmployeeModel loggedInUser = (EmployeeModel)result.Result;
                SetSessionString("current_user_token", loggedInUser.Token);
                SetSessionString("current_user_email", loggedInUser.Email);
                SetSessionString("current_user_role", loggedInUser.Role);

                return result;
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        private void SetSessionString(string name, string property)
        {
            HttpContext.Session.SetString(name, property);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
