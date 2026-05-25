using GymSystem.BLL.Services.Interfaces;
using GymSystem.DAL.UnitOfWork.Interfaces;
using GymSystem.Models.DTOs;
using GymSystem.Models.Entities;


namespace GymSystem.BLL.Services.Implementations
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IUnitOfWork _uow;

        public AttendanceService(IUnitOfWork uow) => _uow = uow;

        public async Task<IEnumerable<AttendanceRecordDto>> GetTodayAsync()
        {
            var records = await _uow.Attendance.GetTodayAsync();
            return records.Select(MapToDto);
        }

        public async Task<IEnumerable<AttendanceRecordDto>> GetByMemberAsync(int memberId)
        {
            var records = await _uow.Attendance.GetByMemberAsync(memberId);
            return records.Select(MapToDto);
        }

        public async Task<string> GetMemberNameAsync(int memberId)
        {
            var member = await _uow.Members.GetByIdAsync(memberId);
            return member?.FullName ?? "Unknown";
        }

        public async Task<IEnumerable<AttendanceRecordDto>> GetFilteredAsync(AttendanceFilterDto filter)
        {
            var query = await _uow.Attendance.GetAllAsync();

            if (filter.MemberId.HasValue)
                query = query.Where(a => a.MemberId == filter.MemberId.Value);

            if (filter.FromDate.HasValue)
                query = query.Where(a => a.CheckInTime.Date >= filter.FromDate.Value.Date);

            if (filter.ToDate.HasValue)
                query = query.Where(a => a.CheckInTime.Date <= filter.ToDate.Value.Date);

            return query.OrderByDescending(a => a.CheckInTime)
                       .Select(MapToDto);
        }

        public async Task<CheckInDto> GetCheckInDataAsync()
        {
            var members = await _uow.Members.GetAllAsync();

            return new CheckInDto
            {
                Members = members.Select(m => new LookupItemDto
                {
                    Id = m.Id,
                    Name = m.FullName
                }).ToList()
            };
        }

        public async Task<CheckInResultDto> CheckInAsync(int memberId)
        {
            var member = await _uow.Members.GetByIdAsync(memberId);
            if (member == null)
                return new CheckInResultDto
                {
                    Success = false,
                    Message = "Member not found"
                };

            var openCheckIn = await _uow.Attendance.GetOpenCheckInAsync(memberId);
            if (openCheckIn != null)
                return new CheckInResultDto
                {
                    Success = false,
                    Message = $"Member already checked in at {openCheckIn.CheckInTime:hh:mm tt}"
                };

            var activeSubscription = await _uow.Subscriptions.GetActiveSubscriptionAsync(memberId);
            if (activeSubscription == null || activeSubscription.EndDate.Date < DateTime.Today)
                return new CheckInResultDto
                {
                    Success = false,
                    Message = "Member membership is not active"
                };

            await _uow.Attendance.AddAsync(new AttendanceRecord
            {
                MemberId = memberId,
                CheckInTime = DateTime.Now
            });

            await _uow.SaveChangesAsync();

            return new CheckInResultDto
            {
                Success = true,
                Message = $"Check-in recorded for {member.FullName}"
            };
        }

        public async Task<CheckInResultDto> CheckOutAsync(int attendanceId)
        {
            var record = await _uow.Attendance.GetByIdAsync(attendanceId);

            if (record == null)
                return new CheckInResultDto
                {
                    Success = false,
                    Message = "Attendance record not found"
                };

            if (record.CheckOutTime != null)
                return new CheckInResultDto
                {
                    Success = false,
                    Message = $"Already checked out at {record.CheckOutTime:hh:mm tt}"
                };

            record.CheckOutTime = DateTime.Now;
            _uow.Attendance.Update(record);
            await _uow.SaveChangesAsync();

            var duration = (record.CheckOutTime.Value - record.CheckInTime).TotalMinutes;

            return new CheckInResultDto
            {
                Success = true,
                Message = $"Check-out recorded. Duration: {duration:F0} minutes"
            };
        }

        public async Task<AttendanceStatsDto> GetStatsAsync(int? memberId = null)
        {
            var records = memberId.HasValue
                ? await _uow.Attendance.GetByMemberAsync(memberId.Value)
                : await _uow.Attendance.GetTodayAsync();

            var completedRecords = records.Where(r => r.CheckOutTime.HasValue).ToList();

            return new AttendanceStatsDto
            {
                TotalCheckIns = records.Count(),
                ActiveMembers = records.Count(r => !r.CheckOutTime.HasValue),
                AverageDurationMinutes = completedRecords.Any()
                    ? completedRecords.Average(r => (r.CheckOutTime.Value - r.CheckInTime).TotalMinutes)
                    : 0,
                BusiestDay = records.Any()
                    ? records.GroupBy(r => r.CheckInTime.Date)
                             .OrderByDescending(g => g.Count())
                             .First().Key
                    : null,
                PeakHour = records.Any()
                    ? records.GroupBy(r => r.CheckInTime.Hour)
                             .OrderByDescending(g => g.Count())
                             .First().Key
                    : 0
            };
        }
        private static AttendanceRecordDto MapToDto(AttendanceRecord entity) => new()
        {
            Id = entity.Id,
            MemberId = entity.MemberId,
            MemberName = entity.Member?.FullName ?? "Unknown",
            CheckInTime = entity.CheckInTime,
            CheckOutTime = entity.CheckOutTime
        };
    }
}

