using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AttendenceTracker.Domain.Entity
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public int RoleID { get; set; }
        public DateTime CreatedAt { get; set; }
        public Role Role { get; set; }
        public ICollection<UserDetails> UserDetails { get; set; }
        public ICollection<Attendance> Attendances { get; set; }
        public ICollection<Attendance> RecordedBy { get; set; }

    }
}