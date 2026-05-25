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

        public string StatusBadgeClass => IsOpen ? "bg-success bg-opacity-10 text-success" : "bg-secondary bg-opacity-10 text-secondary";
        public string StatusText => IsOpen ? "Active" : "Completed";
        public string StatusIcon => IsOpen ? "bi-person-check-fill" : "bi-clock-history";

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
