namespace GymSystem.Web.ViewModels.Dashboard
{
    public class DashboardViewModel
    {
        // Stats cards
        public int TotalMembers { get; set; } = 150;
        public int ActiveSubscriptions { get; set; } = 0;
        public int TodayAttendance { get; set; } = 0;
        public int TotalClasses { get; set; } = 0;

        // Alerts
        //public IEnumerable<ExpiringSubscriptionViewModel> ExpiringSubscriptions { get; set; }
        //    = new List<ExpiringSubscriptionViewModel>();

        //// Today's activity
        //public IEnumerable<TodayAttendanceViewModel> TodayCheckIns { get; set; }
        //    = new List<TodayAttendanceViewModel>();
    }
}
