using Application.DTOs;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace UsertestWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] RegisterRequest request)
        {
            try
            {
                var response = await _authService.RegisterAsync(request);
                return Ok(new { description = "User registered successfully", response });
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { description = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { description = "An error occurred during registration", error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var response = await _authService.LoginAsync(request);
                return Ok(new { description = "Login successful", response });
            }
            catch (ApplicationException ex)
            {
                return Unauthorized(new { description = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { description = "An error occurred during login", error = ex.Message });
            }
        }
    }
}
