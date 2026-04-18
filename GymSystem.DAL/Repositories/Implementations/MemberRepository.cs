
using GymSystem.DAL.Data;
using GymSystem.DAL.Repositories.Interfaces;
using GymSystem.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.DAL.Repositories.Implementations
{
    internal class MemberRepository : GenericRepository<Member>, IMemberRepository
    {
        public MemberRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Member>> GetAllWithDetailsAsync()
            => await _dbSet
                .Include(m => m.Trainer)
                .Include(m => m.Subscriptions)
                    .ThenInclude(s => s.Plan)
                .ToListAsync();

        public async Task<IEnumerable<Member>> GetByTrainerAsync(int trainerId)
            => await _dbSet
                .Where(m => m.TrainerId == trainerId)
                .Include(m => m.Subscriptions)
                    .ThenInclude(s => s.Plan)
                .ToListAsync();

        public async Task<Member?> GetWithDetailsAsync(int id)
            => await _dbSet
                    .Include(m => m.Trainer)
                    .Include(m => m.Subscriptions)
                        .ThenInclude(s => s.Plan)
                    .Include(m => m.ClassEnrollments)
                        .ThenInclude(ce => ce.GymClass)
                    .Include(m => m.AttendanceRecords)
                    .FirstOrDefaultAsync(m => m.Id == id);
    }
}
