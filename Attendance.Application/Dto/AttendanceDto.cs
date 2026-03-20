namespace AttendanceTracker.Application.DTOs
{
    public class AttendanceDTO
    {
        public int UserId { get; set; }
        public int RecordedBy { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Course { get; set; } = string.Empty;
    }
}