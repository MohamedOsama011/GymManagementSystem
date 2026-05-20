using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.Models.DTOs
{
    public class DashboardDto
    {
        // Stats cards
        public int TotalMembers { get; set; }
        public int ActiveSubscriptions { get; set; }
        public int TodayAttendance { get; set; }
        public int TotalClasses { get; set; }

        // Alerts
        public List<ExpiringSubscriptionDto> ExpiringSubscriptions { get; set; } = new();

        // Today's activity
        public List<TodayAttendanceDto> TodayCheckIns { get; set; } = new();
    }
    public class ExpiringSubscriptionDto
    {
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public string PlanName { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class TodayAttendanceDto
    {
        public int AttendanceId { get; set; }
        public string MemberName { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
    }
}
