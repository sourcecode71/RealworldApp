using Application.Core.Employees;
using Application.DTOs;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistance.Context;
using PMG.Data.Repository.Employee;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Web.Services;

namespace Web.ApiControllers
{

    public class EmployeeController : BaseApiController
    {
        private readonly UserManager<Employee> _userManager;
        private readonly SignInManager<Employee> _signInManager;
        private readonly DataContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly TokenService _tokenService;
        private readonly IEmployeeRepository _empRepository;

        public EmployeeController(DataContext context, UserManager<Employee> userManager, SignInManager<Employee> signInManager,
         RoleManager<IdentityRole> roleManager, TokenService tokenService, IEmployeeRepository empRepository)
        {
            _tokenService = tokenService;
            _roleManager = roleManager;
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _empRepository = empRepository;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<EmployeeDto>>> GetEmployees()
        {
            return Ok(await Mediator.Send(new ListEmployees.Query()));
        }

        [Authorize]
        [HttpGet("byEmail")]
        public async Task<ActionResult<EmployeeDto>> GetEmployeeByEmail(string email)
        {
            return Ok(await Mediator.Send(new GetEmployeeByEmail.Query { Email = email }));
        }

        [Authorize]
        [HttpDelete]
        public async Task<ActionResult> DeleteEmployee(string email)
        {
            return Ok(await Mediator.Send(new DeleteEmployee.Command { Email = email }));
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<EmployeeDto>> Login(LoginDto loginDto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(loginDto.Email);

                if (user == null) return Unauthorized();

                var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

                if (result.Succeeded)
                {
                    IList<string> roles = await _userManager.GetRolesAsync(user);
                    return CreateEmployeeObject(user, roles.Count > 0 ? roles[0] : string.Empty);
                }

                return Unauthorized();
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }



        [Authorize]
        [HttpPost("Logout")]
        public async Task<bool> Logout(EmployeeDto loggedInUser)
        {
            if (loggedInUser == null)
            {
                return false;
            }

            Employee user = await _userManager.FindByEmailAsync(loggedInUser.Email);

            if (user == null)
            {
                return false;
            }

            await _signInManager.SignOutAsync();

            return true;
        }

        [HttpPost("create-role")]
        public async Task<bool> CreateRole(RegisterDto registerDto)
        {
            bool existRole = await _roleManager.RoleExistsAsync(registerDto.Role);


            IdentityRole role = new IdentityRole();
            role.Name = registerDto.Role;

            var result = await _roleManager.CreateAsync(role);

            return result.Succeeded == true;
        }

        [HttpGet("all-role")]
        public async Task<ActionResult<List<RolesDTO>>> GetAllRoles()
        {
            var Roles = await _context.Roles.Select(p => new RolesDTO
            {
                Id = p.Id,
                Name = p.Name,
            }).ToListAsync();

            return Roles;
        }


        [HttpPost("register")]
        public async Task<ActionResult<EmployeeDto>> Register(RegisterDto registerDto)
        {
            try
            {
                if (await _userManager.Users.AnyAsync(x => x.Email == registerDto.Email))
                {
                    return BadRequest("Email taken");
                }

                var user = new Employee
                {
                    Name = registerDto.Name,
                    Email = registerDto.Email,
                    UserName = registerDto.Email,
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    PhoneNumber = registerDto.Phone,
                };

                var result = await _userManager.CreateAsync(user, registerDto.Password);

                if (result.Succeeded)
                {
                    bool existRole = await _roleManager.RoleExistsAsync(registerDto.Role);

                    if (!existRole)
                    {
                        IdentityRole role = new IdentityRole();
                        role.Name = registerDto.Role;

                        await _roleManager.CreateAsync(role);
                    }

                    await _userManager.AddToRoleAsync(user, registerDto.Role);

                    IList<string> roles = await _userManager.GetRolesAsync(user);

                    var usrResp = CreateEmployeeObject(user, roles.Count > 0 ? roles[0] : string.Empty);

                    // return Ok(usrResp);

                }

                return Ok(result);
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        }

        [HttpPut("change-password")]
        public async Task<ActionResult> UpdatePassword([FromBody] EmployeeDto pDTO)
        {
            string userId = HttpContext.Session.GetString("current_user_id");
            string Email = HttpContext.Session.GetString("current_user_email");

            var user = new Employee
            {
                Id = userId,
                Email = Email,
            };

            var result = await _userManager.ChangePasswordAsync(user, pDTO.CurrentPassword,pDTO.NewPassword);

            return Ok(result);
        }


        [AllowAnonymous]
        [HttpGet("current")]
        public async Task<ActionResult<EmployeeDto>> GetCurrentUser()
        {
            var user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));

            return CreateEmployeeObject(user, null);
        }

        private EmployeeDto CreateEmployeeObject(Employee user, string role)
        {
            return new EmployeeDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = role,
                Token = _tokenService.CreateToken(user, role)
            };
        }

        [HttpGet("get-all-employees-names")]
        public async Task<ActionResult<List<string>>> GetEnployeesNames()
        {
            var result = await Mediator.Send(new GetAllEmployees.Query());
            return result.ToList().Select(x => x.Email).ToList();
        }

        [HttpGet("all-active-employee")]
        public async Task<ActionResult<List<EmployeeDto>>> GetAllEmployes()
        {
            var empDto = await _empRepository.GetAllActiveEmployee();
            return empDto;
        }

        [Authorize]
        [HttpGet("activity-by-employee")]
        public async Task<ActionResult<List<ProjectActivityDto>>> GetActivitiesForEmployee(string email)
        {
            return Ok(await Mediator.Send(new GetActivitiesForEmployee.Query { Email = email }));
        }

        [HttpGet("activity-by-project")]
        public async Task<ActionResult<List<ProjectActivityDto>>> GetActivitiesForProject(int id)
        {
            return Ok(await Mediator.Send(new GetActivitiesForProject.Query { SelfProjectId = id }));
        }

        [HttpGet("all-emp-project")]
        public async Task<ActionResult<List<EpmProjectsDto>>> GetEmpProjects(string EmpId)
        {
            var PempDto = await _empRepository.GetEmpProjects(EmpId);
            return PempDto;
        }

        [HttpGet("all-emp-workorder")]
        public async Task<ActionResult<List<EpmProjectsDto>>> GetEmpWorkOrders(string EmpId)
        {
            var WrkDto = await _empRepository.GetEmpWorkOrder(EmpId);
            return WrkDto;
        }


        [HttpGet("workorder/emp-hour-summery")]
        public async Task<ActionResult<List<HourslogDto>>> GethourLogSummery(string wrkId)
        {
            var empDto = await _empRepository.EmployeHourLogSummery(wrkId);
            return empDto;
        }

        [HttpGet("workorder/emp-hour-details")]
        public async Task<ActionResult<List<HourslogDto>>> GethourLogDetails(string wrkId)
        {
            var empDto = await _empRepository.EmployeHourLogDetails(wrkId);
            return empDto;
        }


        [HttpGet("all-hour-log/summery")]
        public async Task<ActionResult<List<HourslogDto>>> GetHourLogsSummeryAll()
        {
            var empDto = await _empRepository.EmployeHourLogSummeryAll();
            return empDto;
        }

        [HttpGet("emp-wise-hour")]
        public async Task<ActionResult<List<HourslogDto>>> GetEmpWisehourLogs(string EmpId)
        {
            var empDto = await _empRepository.GetEmpWisehourLogs(EmpId);
            return empDto;
        }

        [HttpGet("emp-ot-hour")]
        public async Task<ActionResult<List<HourslogDto>>> GetEmpWisehourLogs(string EmpId, string wrkId)
        {
            var empDto = await _empRepository.GetEmpWiseWrkOThourLogs(EmpId, wrkId);
            return empDto;
        }


        [HttpPut("emp-wrk-status")]
        public async Task<ActionResult<bool>> PutEmpStatus([FromBody] HourslogDto dto)
        {
            var empDto = await _empRepository.UpdateEmaployeeAssignState(dto);
            return empDto;
        }


        [HttpPut("set-emp-wrk")]
        public async Task<ActionResult<bool>> SetEmployeeForWrk([FromBody] HourslogDto dto)
        {
            var empDto = await _empRepository.SetupEmployeeForWrk(dto);
            return empDto;
        }

        [HttpGet("load-hourlogs-emp")]
        public async Task<ActionResult<List<EmployeeDto>>> GetHrsLogEmployes()
        {
            var empDto = await _empRepository.GetAllActiveEmployee();
            var filterEmp = empDto.Where(p => p.Role == "Drawing" || p.Role == "Engineering").Select(p => p).ToList();
            return filterEmp;
        }
    }
}