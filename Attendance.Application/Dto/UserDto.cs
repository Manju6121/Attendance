using System.ComponentModel.DataAnnotations;

namespace AttendanceTracker.Application.DTOs
{
    public class UserDTO
    {
        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public int RoleID { get; set; }
    }
}