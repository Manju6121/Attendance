using AttendanceTracker.Application.DTOs;
using AttendanceTracker.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AttendenceTracker.Domain.Entity;

namespace AttendanceTracker.API.Controllers
{
    [Route("api/attendance")]
    [ApiController]
    [Authorize]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _attendanceService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var attendance = await _attendanceService.GetByIdAsync(id);
            if (attendance == null) return NotFound();

            return Ok(attendance);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AttendanceDTO dto)
        {
            var attendance = new AttendenceTracker.Domain.Entity.Attendance
            {
                UserId = dto.UserId,
                RecordedBy = dto.RecordedBy,
                Date = dto.Date,
                Status = dto.Status,
                Course = dto.Course
            };

            var result = await _attendanceService.CreateAsync(attendance);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, AttendanceDTO dto)
        {
            var attendance = new AttendenceTracker.Domain.Entity.Attendance
            {
                AttendanceId = id,
                UserId = dto.UserId,
                RecordedBy = dto.RecordedBy,
                Date = dto.Date,
                Status = dto.Status,
                Course = dto.Course
            };

            var result = await _attendanceService.UpdateAsync(id, attendance);
            if (result == null) return NotFound();

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _attendanceService.DeleteAsync(id);
            if (!deleted) return NotFound();

            return Ok("Deleted Successfully");
        }
    }
}