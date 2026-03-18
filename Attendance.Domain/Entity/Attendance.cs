using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AttendenceTracker.Domain.Entity
{
    //public class Attendance
    //{
    //    [Key]
    //    public int AttendanceId { get; set; }
    //    public int UserId { get; set; }
    //    public DateTime Date { get; set; }
    //    public string Status { get; set; }
    //    public string Course { get; set; }
    //    public int RecordedBy { get; set; }
    //    //[ForeignKey("UserID")]
    //    public User User { get; set; }
    //    //[ForeignKey("RecordedBy")]
    //    public User RecordedUser { get; set; }
    //}

    public class Attendance
    {
        [Key]
        public int AttendanceId { get; set; }

        public int UserId { get; set; }      // Student
        public int RecordedBy { get; set; }  // Faculty

        public DateTime Date { get; set; }
        public string Status { get; set; }
        public string Course { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [ForeignKey("RecordedBy")]
        public User RecordedUser { get; set; }
    }
}