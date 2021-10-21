using System.Threading.Tasks;
using API.DTOs;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{

    public class EmployeeController : BaseApiController
    {
        private readonly UserManager<Employee> _userManager;
        private readonly SignInManager<Employee> _signInManager;
        public EmployeeController(UserManager<Employee> userManager, SignInManager<Employee> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        [HttpPost("login")]
        public async Task<ActionResult<EmployeeDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if(user == null) return Unauthorized();

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if(result.Succeeded)
            {
                return new EmployeeDto
                {
                    Name = user.Name,
                    Email = user.Email
                };
            }

            return Ok();
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
                IsAdmin = false
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (result.Succeeded)
            {
                return new EmployeeDto
                {
                    Name = user.Name,
                    Email = user.Email
                };
            }

            return BadRequest("Problem registring user");
        }
    }
}