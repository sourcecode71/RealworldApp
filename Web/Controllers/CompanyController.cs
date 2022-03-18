using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class CompanyController : Controller
    {
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
                    return View();
                }
                else
                {
                    return RedirectToAction("Forbidden", "Home");
                }
            }

            return RedirectToAction("Login", "Home");
        }

        public IActionResult Client()
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
                    return RedirectToAction("Forbidden", "Home");
                }
            }

            return RedirectToAction("Login", "Home");
        }

        public IActionResult Invoice()
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
                    return RedirectToAction("Forbidden", "Home");
                }
            }

            return RedirectToAction("Login", "Home");
        }

        public IActionResult Payment()
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
                    return RedirectToAction("Forbidden", "Home");
                }
            }

            return RedirectToAction("Login", "Home");
        }

        public IActionResult HourLogs()
        {
            string currentEmail = HttpContext.Session.GetString("current_user_email");

            if (!string.IsNullOrEmpty(currentEmail))
            {
                string currentRole = HttpContext.Session.GetString("current_user_role");

                if (string.IsNullOrEmpty(currentRole))
                {
                    return RedirectToAction("Login", "Home");
                }

                if (currentRole == "Admin" || currentRole == "Engineering" || currentRole == "Drawing")
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Forbidden", "Home");
                }
            }

            return RedirectToAction("Login", "Home");
        }


        public IActionResult HourlogsHistory()
        {
            return View();
        }
    }
}
