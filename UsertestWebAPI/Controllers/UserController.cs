using Application.DTOs;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace UsertestWebAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Require authentication for all endpoints
    public class UserController : BaseController<User,UserResponse,UserModel>
    {
        private readonly IService<User, UserResponse, UserModel> _service;
        public UserController(IService<User, UserResponse, UserModel> service)
: base(service)
        {
            _service = service;
        }
        
        [HttpGet("active")]
        [Authorize(Policy = "ViewUsers")] // Require ViewUsers permission
        public async Task<IActionResult> GetActive()
        {
            var response = await _service.GetAllAsync();
            return Ok(new {description = "Returned All Users", response });
        }
    }
}
