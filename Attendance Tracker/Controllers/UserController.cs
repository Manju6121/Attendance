using AttendanceTracker.Application.DTOs;
using AttendanceTracker.Application.Interfaces;
using AttendenceTracker.Domain.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceTracker.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

      

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _service.GetByIdAsync(id);
            if (user == null) return NotFound();

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _service.LoginAsync(dto.UserName, dto.Password);
            if (result == null)
                return Unauthorized("Invalid username or password");

            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, UserUpdateDto dto)
        {
            var user = new User
            {
                UserName = dto.UserName,
                Email = dto.Email,
                RoleID = dto.RoleID
            };

            var result = await _service.UpdateAsync(id, user, dto.Password);
            if (result == null) return NotFound();

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();

            return Ok("Deleted Successfully");
        }
        [HttpPost]
        public async Task<IActionResult> Create(UserUpdateDto dto)
        {
            var user = new User
            {
                UserName = dto.UserName,
                Email = dto.Email,
                RoleID = dto.RoleID,
                CreatedAt = DateTime.Now,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            var result = await _service.CreateAsync(user);
            return Ok(result);
        }

    }
}