using Application.DTOs;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
namespace UsertestWebAPI.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class UserController : BaseController<User,UserResponse,UserModel>
    {
        private readonly IService<User, UserResponse, UserModel> _service;
        public UserController(IService<User, UserResponse, UserModel> service)
: base(service)
        {
            _service = service;
        }
        [HttpGet("active")]
        public async Task<IActionResult> GetActive()
        {
            var response = await _service.GetAllAsync();
            return Ok(new {description = "Returned All Users", response });
        }
    }
}
