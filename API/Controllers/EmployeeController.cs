using Application.DTOs;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistance.Context;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Core.Employees;
using System.Security.Claims;
using System.Linq;
using Domain.Enums;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace API.Controllers
{

    public class EmployeeController : BaseApiController
    {
        private readonly UserManager<Employee> _userManager;
        private readonly SignInManager<Employee> _signInManager;
        private readonly DataContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly TokenService _tokenService;

        public EmployeeController(DataContext context, UserManager<Employee> userManager, SignInManager<Employee> signInManager,
         RoleManager<IdentityRole> roleManager, TokenService tokenService)
        {
            _tokenService = tokenService;
            _roleManager = roleManager;
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        
       [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<ActionResult<List<EmployeeDto>>> GetEmployees()
        {
            return Ok(await Mediator.Send(new ListEmployees.Query()));
        }
        
        [HttpGet("byEmail")]
        public async Task<ActionResult<EmployeeDto>> GetEmployeeByEmail(string email)
        {
            return Ok(await Mediator.Send(new GetEmployeeByEmail.Query{Email = email}));
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteEmployee(string email)
        {
            return Ok(await Mediator.Send(new DeleteEmployee.Command { Email = email }));
        }

        [HttpPost("login")]
        public async Task<ActionResult<EmployeeDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null) return Unauthorized();

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (result.Succeeded)
            {
                return CreateEmployeeObject(user);
            }

            return Unauthorized();
        }
    

        [HttpPost("register")]
        public async Task<ActionResult<EmployeeDto>> Register(RegisterDto registerDto)
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
                IsAdmin = registerDto.IsAdmin
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (result.Succeeded)
            {
                await CreateRole(user, user.IsAdmin);
                return CreateEmployeeObject(user);
            }

            return BadRequest("Problem registring user");
        }

        [HttpPost("activity")]
        public async Task<ActionResult> AddActivity([FromForm] string employeeEmail, [FromForm] int selfProjectId,
        [FromForm] double duration, [FromForm] string comment, [FromForm] ProjectStatus status, 
        [FromForm] string statusComment)
        {
            return Ok(await Mediator.Send(new AddActivity.Command
            {
                EmployeeEmail = employeeEmail,
                SelfProjectId = selfProjectId,
                Duration = duration,
                Comment = comment,
                Status = status,
                StatusComment = statusComment
            }));
        }

        [HttpGet("current")]
        public async Task<ActionResult<EmployeeDto>> GetCurrentUser()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            Employee user = await _context.Employees.FirstOrDefaultAsync(x => x.Id == userId);
            return CreateEmployeeObject(user);
        }

        private EmployeeDto CreateEmployeeObject(Employee user)
        {
            return new EmployeeDto
            {
                Name = user.Name,
                Email = user.Email,
                Token = _tokenService.CreateToken(user)
            };
        }

        private async Task CreateRole(Employee employee, bool isAdmin)
        {
            if (isAdmin)
            {
                var role = new IdentityRole();
                role.Name = "Admin";
                await _roleManager.CreateAsync(role);

                var result1 = await _userManager.AddToRoleAsync(employee, "Admin");

                if (!result1.Succeeded) throw new System.Exception("Error!");
            }
            else
            {
                var role = new IdentityRole();
                role.Name = "Employee";
                await _roleManager.CreateAsync(role);

                var result1 = await _userManager.AddToRoleAsync(employee, "Employee");

                if (!result1.Succeeded) throw new System.Exception("Error!");
            }
        }

        [HttpGet("get-all-employees-names")]
        public async Task<ActionResult<List<string>>> GetEnployeesNames()
        {
            var result = await Mediator.Send(new GetAllEmployees.Query());
            return result.ToList().Select(x => x.Name).ToList();
        }
    }
}