namespace GymSystem.Web.ViewModels.Dashboard
{
    public class TodayAttendanceViewModel
    {
        public int AttendanceId { get; set; }
        public string MemberName { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }

        // UI Logic
        public bool IsOpen => CheckOutTime == null;

        public string StatusBadgeClass => IsOpen ? "bg-success" : "bg-secondary";
        public string StatusText => IsOpen ? "Active" : "Completed";
        public string StatusIcon => IsOpen ? "fa-user-check" : "fa-user-clock";

        public string CheckInTimeFormatted => CheckInTime.ToString("hh:mm tt");
        public string CheckOutTimeFormatted => CheckOutTime?.ToString("hh:mm tt") ?? "-";

        public string Duration
        {
            get
            {
                if (!CheckOutTime.HasValue) return "In progress";

                var duration = CheckOutTime.Value - CheckInTime;
                return duration.TotalHours >= 1
                    ? $"{(int)duration.TotalHours}h {duration.Minutes}m"
                    : $"{duration.Minutes}m";
            }
        }
    }
}
