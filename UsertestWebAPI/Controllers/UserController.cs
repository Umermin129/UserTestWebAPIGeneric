using Application.Services.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
namespace UsertestWebAPI.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class UserController(IUserService userService) : Controller
    {
        private readonly IUserService _userService = userService;

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserModel  user)
        {
            var response = await _userService.CreateUser(user);
            return Ok(new
            {
                StatusCode = 200,
                description = "User Created Successfully",
                name = response.Name
        });
        }
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            return Ok(new
            {
                StatusCode = 200,
                description = "Returned response successfully!"

            });
        }
    }
}
