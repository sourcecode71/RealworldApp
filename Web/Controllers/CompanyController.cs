using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class CompanyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Client()
        {
            return View();
        }

        public IActionResult Invoice()
        {
            return View();
        }

        public IActionResult Payment()
        {
            return View();
        }

        public IActionResult HourLogs()
        {
            return View();
        }
    }
}
