using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistance.Context;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Web.Services;
using Application.DTOs;
using Application.Core.Employees;

namespace Web.ApiControllers
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

        [HttpGet("all-role")]
        public async Task<ActionResult<List<RolesDTO>>> GetAllRoles()
        {
            var Roles = await _context.Roles.Select(p=> new RolesDTO
            {
                Id = p.Id,
                Name = p.Name,
            }).ToListAsync();

            return Roles;
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
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Phone = registerDto.Phone,
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

                return CreateEmployeeObject(user, roles.Count > 0 ? roles[0] : string.Empty);
            }

            return BadRequest("Problem registring user");
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
    }
}