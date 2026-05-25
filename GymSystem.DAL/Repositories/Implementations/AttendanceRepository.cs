using GymSystem.DAL.Data;
using GymSystem.DAL.Repositories.Interfaces;
using GymSystem.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repositories.Implementations
{
    public class AttendanceRepository : GenericRepository<AttendanceRecord>, IAttendanceRepository
    {
        public AttendanceRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<AttendanceRecord>> GetByMemberAsync(int memberId)
            => await _dbSet
                .Include(a => a.Member)
                .Where(a => a.MemberId == memberId)
                .OrderByDescending(a => a.CheckInTime)
                .ToListAsync();

        public async Task<IEnumerable<AttendanceRecord>> GetTodayAsync()
            => await _dbSet
                .Include(a => a.Member)
                .Where(a => a.CheckInTime.Date == DateTime.Today)
                .OrderByDescending(a => a.CheckInTime)
                .ToListAsync();

        public async Task<AttendanceRecord?> GetOpenCheckInAsync(int memberId)
            => await _dbSet
                .Where(a => a.MemberId == memberId && a.CheckOutTime == null)
                .OrderByDescending(a => a.CheckInTime)
                .FirstOrDefaultAsync();
    }
}
