using GymSystem.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repositories.Interfaces
{
    public interface IAttendanceRepository : IGenericRepository<AttendanceRecord>
    {
        Task<IEnumerable<AttendanceRecord>> GetByMemberAsync(int memberId);
        Task<IEnumerable<AttendanceRecord>> GetTodayAsync();
        Task<AttendanceRecord?> GetOpenCheckInAsync(int memberId);
    }
}
