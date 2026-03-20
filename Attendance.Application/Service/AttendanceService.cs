using AttendanceTracker.Application.Interfaces;
using AttendenceTracker.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace AttendanceTracker.Application.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly AttendanceDbContext _context;

        public AttendanceService(AttendanceDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AttendenceTracker.Domain.Entity.Attendance>> GetAllAsync()
        {
            return await _context.Attendances
                .Include(a => a.User)
                .Include(a => a.RecordedUser)
                .ToListAsync();
        }

        public async Task<AttendenceTracker.Domain.Entity.Attendance?> GetByIdAsync(int id)
        {
            return await _context.Attendances
                .Include(a => a.User)
                .Include(a => a.RecordedUser)
                .FirstOrDefaultAsync(a => a.AttendanceId == id);
        }

        public async Task<AttendenceTracker.Domain.Entity.Attendance> CreateAsync(AttendenceTracker.Domain.Entity.Attendance attendance)
        {
            _context.Attendances.Add(attendance);
            await _context.SaveChangesAsync();
            return attendance;
        }

        public async Task<AttendenceTracker.Domain.Entity.Attendance?> UpdateAsync(int id, AttendenceTracker.Domain.Entity.Attendance attendance)
        {
            var existing = await _context.Attendances.FindAsync(id);
            if (existing == null) return null;

            existing.UserId = attendance.UserId;
            existing.RecordedBy = attendance.RecordedBy;
            existing.Date = attendance.Date;
            existing.Status = attendance.Status;
            existing.Course = attendance.Course;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var attendance = await _context.Attendances.FindAsync(id);
            if (attendance == null) return false;

            _context.Attendances.Remove(attendance);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}