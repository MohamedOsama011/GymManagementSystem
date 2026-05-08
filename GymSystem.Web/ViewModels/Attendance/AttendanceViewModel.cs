namespace GymSystem.Web.ViewModels.Attendance
{
    public class AttendanceViewModel
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }

        public bool IsOpen => CheckOutTime == null;

        public string StatusBadgeClass => IsOpen ? "bg-success" : "bg-secondary";
        public string StatusText => IsOpen ? "Active" : "Completed";
        public string StatusIcon => IsOpen ? "fa-user-check" : "fa-user-clock";

        public string Duration
        {
            get
            {
                if (!CheckOutTime.HasValue)
                    return "Still in gym";

                var duration = CheckOutTime.Value - CheckInTime;

                if (duration.TotalHours >= 1)
                    return $"{(int)duration.TotalHours}h {duration.Minutes}m";

                return $"{duration.Minutes} min";
            }
        }

        public string FormattedCheckIn => CheckInTime.ToString("hh:mm tt");
        public string FormattedCheckOut => CheckOutTime?.ToString("hh:mm tt") ?? "-";
        public string FormattedDate => CheckInTime.ToString("MMM dd, yyyy");

        public string RowClass => IsOpen ? "table-success" : "";
    }
}
