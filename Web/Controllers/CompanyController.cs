using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class CompanyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(AuthenticationSchemes = "Cookies")]
        public IActionResult Client()
        {
            return View();
        }

        [Authorize(AuthenticationSchemes = "Cookies")]
        public IActionResult Invoice()
        {
            return View();
        }

        [Authorize(AuthenticationSchemes = "Cookies")]
        public IActionResult Payment()
        {
            return View();
        }

        [Authorize(AuthenticationSchemes = "Cookies")]
        public IActionResult HourLogs()
        {
            return View();
        }
    }
}
