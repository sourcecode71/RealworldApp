using Application.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PMG.Data.Repository.Employee;
using System.Threading.Tasks;

namespace Web.ApiContollers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IEmployeeRepository _empRepository;

        public ProfileController(IEmployeeRepository employeeRepository)
        {
            _empRepository = employeeRepository;
        }

        [HttpGet("my-profile")]
        public async Task<ActionResult> GetProfile()
        {
            string userId = HttpContext.Session.GetString("current_user_id");
            EmployeeDto employeeDto = await _empRepository.GetEmployeeById(userId);
            return Ok(employeeDto);
        }

       
    }
}
