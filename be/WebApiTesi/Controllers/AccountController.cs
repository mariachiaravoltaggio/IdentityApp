using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApiTesi.DTOs.Account;
using WebApiTesi.Models;
using WebApiTesi.Services;

namespace WebApiTesi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly JWTService _jwtService;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public AccountController(
            JWTService jwtService, 
            SignInManager<User> signInManager, 
            UserManager<User> userManager)
        {
            _jwtService = jwtService;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [Authorize]
        [HttpGet("refresh-user-token")]
        public async Task<ActionResult<UserDTO>> RefreshUserToken()
        {
            var username = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _userManager.FindByNameAsync(username);

            return CreateApplicationUserDTO(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            var user = await _userManager.FindByNameAsync(loginDTO.UserName);
            if (user == null) return Unauthorized("invalid UserName or Password");
            if(!user.EmailConfirmed) return Unauthorized("Email not confirmed");

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);
            if (!result.Succeeded)
            {
                return Unauthorized("invalid UserName or Password");
            }

            return CreateApplicationUserDTO(user);
            
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            if (await CheckEmailExistsAsync(registerDTO.Email))
            {
                return BadRequest($"This Email {registerDTO.Email} already exists");
            }
            var userToAdd = new User
            {
                FirstName = registerDTO.FirstName.ToLower(),
                LastName = registerDTO.LastName.ToLower(),
                Email = registerDTO.Email.ToLower(),
                UserName = registerDTO.Email.ToLower(),
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(userToAdd, registerDTO.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            return Ok("Your account is created");
        }
        private UserDTO CreateApplicationUserDTO(User user)
        {
            return new UserDTO
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                JWT = _jwtService.CreateJWT(user)
            };
        }   

        private async Task<bool> CheckEmailExistsAsync(string email)
        {
            return await _userManager.Users.AnyAsync(x => x.Email == email.ToLower());
        }
    }
}
