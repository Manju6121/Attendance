namespace AttendanceTracker.Application.DTOs
{
    public class UserDetailsDTO
    {
        public int UserID { get; set; }
        public string FullName { get; set; } = string.Empty;
        public DateTime DOB { get; set; }
        public string? Gender { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? Department { get; set; }
        public int Year { get; set; }
    }
}