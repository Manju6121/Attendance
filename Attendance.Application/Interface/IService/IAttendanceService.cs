using AttendenceTracker.Domain.Entity;

namespace AttendanceTracker.Application.Interfaces
{
    public interface IAttendanceService
    {
        Task<IEnumerable<AttendenceTracker.Domain.Entity.Attendance>> GetAllAsync();
        Task<AttendenceTracker.Domain.Entity.Attendance?> GetByIdAsync(int id);
        Task<AttendenceTracker.Domain.Entity.Attendance> CreateAsync(AttendenceTracker.Domain.Entity.Attendance attendance);
        Task<AttendenceTracker.Domain.Entity.Attendance?> UpdateAsync(int id, AttendenceTracker.Domain.Entity.Attendance attendance);
        Task<bool> DeleteAsync(int id);
    }
}