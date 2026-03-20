using System.ComponentModel.DataAnnotations;

namespace AttendanceTracker.Application.DTOs.Role
{
    public class RoleDTO
    {
        [Required]
        public string RoleName { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}