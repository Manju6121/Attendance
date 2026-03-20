using AttendanceTracker.Application.DTOs;
using AttendenceTracker.Domain.Entity;

namespace AttendanceTracker.Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponseDTO>> GetAllAsync();
        Task<UserResponseDTO?> GetByIdAsync(int id);
        Task<UserResponseDTO> CreateAsync(User user);
        Task<UserResponseDTO?> UpdateAsync(int id, User user, string? password = null);
        Task<bool> DeleteAsync(int id);
        Task<LoginResponseDTO?> LoginAsync(string username, string password);
    }
}