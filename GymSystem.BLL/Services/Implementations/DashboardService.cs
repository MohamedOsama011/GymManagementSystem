using GymSystem.BLL.Services.Interfaces;
using GymSystem.DAL.UnitOfWork.Interfaces;
using GymSystem.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Implementations
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _uow;

        public DashboardService(IUnitOfWork uow) => _uow = uow;
        public async Task<DashboardDto> GetDashboardDataAsync()
        {
            var members = await _uow.Members.GetAllWithDetailsAsync();
            var todayRecords = await _uow.Attendance.GetTodayAsync();
            var expiring = await _uow.Subscriptions.GetExpiringSoonAsync(7);
            var allClasses = await _uow.GymClasses.GetAllAsync();

            var memberList = members.ToList();

            var activeSubscriptions = memberList
                .SelectMany(m => m.Subscriptions)
                .Count(s => s.Status == "Active");

            var dashboardDto = new DashboardDto
            {
                TotalMembers = memberList.Count,
                ActiveSubscriptions = activeSubscriptions,
                TodayAttendance = todayRecords.Count(),
                TotalClasses = allClasses.Count(),

                ExpiringSubscriptions = expiring.Select(s => new ExpiringSubscriptionDto
                {
                    MemberId = s.Member.Id,
                    MemberName = s.Member.FullName,
                    PlanName = s.Plan?.Name ?? "Unknown Plan",
                    EndDate = s.EndDate
                }).OrderBy(s => s.EndDate) 
                  .ToList(),

                TodayCheckIns = todayRecords.Select(a => new TodayAttendanceDto
                {
                    AttendanceId = a.Id,
                    MemberName = a.Member?.FullName ?? "Unknown",
                    CheckInTime = a.CheckInTime,
                    CheckOutTime = a.CheckOutTime
                }).OrderByDescending(a => a.CheckInTime) 
                  .ToList()
            };

            return dashboardDto;

        }
    }
}
