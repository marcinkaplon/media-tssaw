using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using IdentityAPI.Models;
using IdentityAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace IdentityAPI.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (loginDto == null || string.IsNullOrEmpty(loginDto.Email) ||
                string.IsNullOrEmpty(loginDto.Password))
                return BadRequest("Invalid login request.");

            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) return Unauthorized(new { Message = "Invalid Email or password" });

            //var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            var result = await _signInManager.PasswordSignInAsync(user.UserName, loginDto.Password, false, false);

            var token = GenerateJwtToken(user);

            if (result.Succeeded)
            {
                return Ok(new { Message = "User logged in successfully", Token = token, userAuthId = user.Id });
            }
            return Unauthorized(new { Message = "Invalid username or password" });
        }

        private string GenerateJwtToken(ApplicationUser user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (registerDto.Password != registerDto.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Passwords do not match.");
                return BadRequest(new
                {
                    Errors = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray())
                });
            }

            var user = new ApplicationUser
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);


            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }

                return BadRequest(new
                {
                    Errors = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray())
                });
            }

            var newUser = await _userManager.FindByEmailAsync(user.Email);

            return Ok(new { Message = "User registered successfully", Id = newUser.Id});
        }

        [HttpPost("delete-account")]
        public async Task<IActionResult> DeleteAccount([FromBody] DeleteAccountDto deleteAccountDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(deleteAccountDto.Email);
            if (user == null)
            {
                return NotFound(new { Message = "User not found." });
            }

            if (deleteAccountDto.Password != deleteAccountDto.ConfirmPassword)
            {
                return BadRequest(new { Message = "Passwords do not match." });
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, deleteAccountDto.Password);
            if (!isPasswordValid)
            {
                return BadRequest(new { Message = "Invalid password." });
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    Message = "Error while deleting account",
                    Errors = result.Errors.Select(e => e.Description)
                });
            }

            return Ok(new { Message = "Account deleted successfully" });
        }
    }
}

