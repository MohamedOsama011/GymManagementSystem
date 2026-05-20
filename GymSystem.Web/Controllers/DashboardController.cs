using GymSystem.BLL.Services.Interfaces;
using GymSystem.Web.ViewModels.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Web.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public async Task<IActionResult> Index()
        {
            var dto = await _dashboardService.GetDashboardDataAsync();

            
            var viewModel = new DashboardViewModel
            {
                TotalMembers = dto.TotalMembers,
                ActiveSubscriptions = dto.ActiveSubscriptions,
                TodayAttendance = dto.TodayAttendance,
                TotalClasses = dto.TotalClasses,

                ExpiringSubscriptions = dto.ExpiringSubscriptions.Select(s => new ExpiringSubscriptionViewModel
                {
                    MemberId = s.MemberId,
                    MemberName = s.MemberName,
                    PlanName = s.PlanName,
                    EndDate = s.EndDate
                }),

                TodayCheckIns = dto.TodayCheckIns.Select(a => new TodayAttendanceViewModel
                {
                    AttendanceId = a.AttendanceId,
                    MemberName = a.MemberName,
                    CheckInTime = a.CheckInTime,
                    CheckOutTime = a.CheckOutTime
                })
            };

            return View(viewModel);
        }
    }
}
