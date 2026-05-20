namespace GymSystem.Web.ViewModels.Dashboard
{
    public class DashboardViewModel
    {
        // Stats cards
        public int TotalMembers { get; set; }
        public int ActiveSubscriptions { get; set; }
        public int TodayAttendance { get; set; }
        public int TotalClasses { get; set; }

        // UI Computed Properties
        public string MemberGrowthIndicator => TotalMembers > 0 ? "↑" : "→";
        public string SubscriptionRate => TotalMembers > 0
            ? $"{(double)ActiveSubscriptions / TotalMembers * 100:F1}%"
            : "0%";
        public string AttendanceRate => TotalMembers > 0
            ? $"{(double)TodayAttendance / TotalMembers * 100:F1}%"
            : "0%";

        // Alerts
        public IEnumerable<ExpiringSubscriptionViewModel> ExpiringSubscriptions { get; set; }
            = new List<ExpiringSubscriptionViewModel>();

        // Today's activity
        public IEnumerable<TodayAttendanceViewModel> TodayCheckIns { get; set; }
            = new List<TodayAttendanceViewModel>();
    }
}
