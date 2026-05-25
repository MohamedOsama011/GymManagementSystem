using GymSystem.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Interfaces
{
    public interface IAttendanceService
    {
        Task<IEnumerable<AttendanceRecordDto>> GetTodayAsync(); 
        Task<IEnumerable<AttendanceRecordDto>> GetByMemberAsync(int memberId);
        Task<string> GetMemberNameAsync(int memberId);
        Task<IEnumerable<AttendanceRecordDto>> GetFilteredAsync(AttendanceFilterDto filter); 
        Task<CheckInDto> GetCheckInDataAsync(); 
        Task<CheckInResultDto> CheckInAsync(int memberId); 
        Task<CheckInResultDto> CheckOutAsync(int attendanceId); 
        Task<AttendanceStatsDto> GetStatsAsync(int? memberId = null); 
    }
    public class AttendanceStatsDto
    {
        public int TotalCheckIns { get; set; }
        public int ActiveMembers { get; set; } 
        public double AverageDurationMinutes { get; set; }
        public DateTime? BusiestDay { get; set; }
        public int PeakHour { get; set; }
    }
}
