using AttendanceTracker.Application.DTOs.Role;
using AttendanceTracker.Application.Interfaces;
using AttendenceTracker.Domain.Entity;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceTracker.API.Controllers
{
    [Route("api/roles")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _roleService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var role = await _roleService.GetByIdAsync(id);
            if (role == null) return NotFound();

            return Ok(role);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleDTO dto)
        {
            var role = new Role
            {
                RoleName = dto.RoleName,
                Description = dto.Description
            };

            var result = await _roleService.CreateAsync(role);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, RoleDTO dto)
        {
            var role = new Role
            {
                RoleID = id,
                RoleName = dto.RoleName,
                Description = dto.Description
            };

            var result = await _roleService.UpdateAsync(id, role);
            if (result == null) return NotFound();

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _roleService.DeleteAsync(id);
            if (!deleted) return NotFound();

            return Ok("Deleted Successfully");
        }
    }
}