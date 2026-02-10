using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;

namespace UsertestWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController<TEntity, TResponse, TCreate> : ControllerBase where TEntity : class
    {
        protected readonly IService<TEntity,TResponse,TCreate> _service;
        protected BaseController(IService<TEntity, TResponse, TCreate> service)
        {
            _service = service;
        }


        [HttpPost]
        public virtual async Task<IActionResult> Create(TCreate model)
        {
            var created = await _service.CreateAsync(model);
            // Attempt to get Id dynamically from response
            var id = (created as dynamic)?.Id ?? Guid.NewGuid();
            return CreatedAtAction(nameof(Get), new { id, description = $"Created new {nameof(TEntity)}" }, created);
        }
        [HttpGet("{id:guid}")]
        public virtual async Task<IActionResult> Get(Guid id)
        {
            var response = await _service.GetAsync(id);
            if (response == null) return NotFound();
        
            return Ok(new {description = $"Successfully Retrieved {typeof(TEntity).Name}", response });
        }
        [HttpGet]
        public virtual async Task<IActionResult> GetAll()
        {
            var response = await _service.GetAllAsync();
            if (response ==null) return NotFound();
            return Ok(new { description = $"Successfully Retrieved all {typeof(TEntity).Name}s", response });
        }
    }
}
