using System.ComponentModel.DataAnnotations;

namespace AttendanceTracker.Application.DTOs
{
    public class UserUpdateDto
    {
        [Required]
        public string UserName { get; set; } = string.Empty;

        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string? Password { get; set; }

        [Required]
        public int RoleID { get; set; }
    }
}