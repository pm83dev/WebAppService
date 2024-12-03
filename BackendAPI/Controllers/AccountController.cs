using Microsoft.AspNetCore.Mvc;
using Application.Dtos;
using Domain.Entities;
using Application.Commands;


namespace BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var token = await _accountService.LoginAsync(loginDto);
            if (token != null)
            {
                return Ok(new { token });
            }
            return Unauthorized();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            
            if (await _accountService.RegisterAsync(registerDto))
            {
                return Ok(new { result = "Registration successful" });
            }
            return BadRequest("Registration failed");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] RegisterDto updatedUserDto)
        {
            if (await _accountService.UpdateUserAsync(id, updatedUserDto))
            {
                return NoContent();
            }
            return NotFound();
        }

        [HttpGet("UserList")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUserList()
        {
            var users = await _accountService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _accountService.LogoutAsync();
            return Ok(new { message = "Logged out" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (await _accountService.DeleteUserAsync(id))
            {
                return NoContent();
            }
            return NotFound();
        }
    }
}
