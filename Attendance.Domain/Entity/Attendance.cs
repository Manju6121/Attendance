using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendenceTracker.Domain.Entity
{
    public class Attendance
    {
        [Key]
        public int AttendanceId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }      // Student

        [ForeignKey("RecordedUser")]
        public int RecordedBy { get; set; }  // Faculty/Admin

        public DateTime Date { get; set; }

        [Required]
        public string Status { get; set; } = string.Empty; // Present / Absent / Late

        [Required]
        public string Course { get; set; } = string.Empty;

        public User? User { get; set; }
        public User? RecordedUser { get; set; }
    }
}