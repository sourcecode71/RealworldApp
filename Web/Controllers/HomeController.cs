using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
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
            string currentEmail = HttpContext.Session.GetString("current_user_email");

            if (!string.IsNullOrEmpty(currentEmail))
            {
                string currentRole = HttpContext.Session.GetString("current_user_role");

                if (string.IsNullOrEmpty(currentRole))
                {
                    return RedirectToAction("Login", "Home");
                }

                return RedirectToAction("Index", "Admin");
            }

            return RedirectToAction("Login", "Home");

            //   return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Forbidden()
        {
            return View();
        }

       public ResultModel Login(EmployeeModel loginUser)
        {
            ResultModel result = _apiService.CallLogin(loginUser).Result;

            if(result.IsSuccess)
            {
                EmployeeModel loggedInUser = (EmployeeModel)result.Result;
                SetSessionString("current_user_token", loggedInUser.Token);
                SetSessionString("current_user_email", loggedInUser.Email);
                SetSessionString("current_user_role", loggedInUser.Role);
                SetSessionString("current_user_name", loggedInUser.Name);
                SetSessionString("current_user_id", loggedInUser.Id);
                
                // TODO : Remove above code and use the following code

                //var claims = new List<Claim>
                //{
                //    new Claim(ClaimTypes.NameIdentifier, loggedInUser.Id.ToString()),
                //    new Claim(ClaimTypes.Name, loggedInUser.Name),
                //    new Claim(ClaimTypes.Role, loggedInUser.Role),
                //    new Claim("token", loggedInUser.Token),
                //};

                //var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                //var principal = new ClaimsPrincipal(identity);

                // HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                //        principal,
                //        new AuthenticationProperties { IsPersistent = true });

            }

            return result;
        }

        [HttpPost]
        public IActionResult Logout()
        {
            string currentToken = HttpContext.Session.GetString("current_user_token");
            string currentEmail = HttpContext.Session.GetString("current_user_email");

            bool isLogout = _apiService.CallLogout(new EmployeeModel { Email = currentEmail, Token = currentToken }).Result;

            HttpContext.Session.Remove("current_user_token");
            HttpContext.Session.Remove("current_user_email");
            HttpContext.Session.Remove("current_user_role");
            HttpContext.Session.Remove("current_user_name");

            return RedirectToAction("Login", "/Home");
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
