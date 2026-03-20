using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendenceTracker.Domain.Entity
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        [ForeignKey("Role")]
        public int RoleID { get; set; }

        public DateTime CreatedAt { get; set; }

        public Role? Role { get; set; }

        public ICollection<UserDetails> UserDetails { get; set; } = new List<UserDetails>();
        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
        public ICollection<Attendance> RecordedAttendances { get; set; } = new List<Attendance>();
    }
}