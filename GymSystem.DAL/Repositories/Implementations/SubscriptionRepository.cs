using GymSystem.DAL.Data;
using GymSystem.DAL.Repositories.Interfaces;
using GymSystem.Models.Entities;
using Microsoft.EntityFrameworkCore;


namespace GymSystem.DAL.Repositories.Implementations
{
    public class SubscriptionRepository : GenericRepository<Subscription>, ISubscriptionRepository
    {
        public SubscriptionRepository(AppDbContext context) : base(context) { }

        public async Task<Subscription?> GetActiveSubscriptionAsync(int memberId)
            => await _dbSet
                .Include(s => s.Plan)
                .Where(s => s.MemberId == memberId && s.Status == "Active")
                .OrderByDescending(s => s.StartDate)
                .FirstOrDefaultAsync();

        public async Task<IEnumerable<Subscription>> GetExpiringSoonAsync(int daysAhead)
        {
            var cutoff = DateTime.Today.AddDays(daysAhead);
            return await _dbSet
                .Include(s => s.Member)
                .Include(s => s.Plan)
                .Where(s => s.Status == "Active" && s.EndDate <= cutoff)
                .OrderBy(s => s.EndDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Subscription>> GetByMemberAsync(int memberId)
            => await _dbSet
                .Include(s => s.Plan)
                .Where(s => s.MemberId == memberId)
                .OrderByDescending(s => s.StartDate)
                .ToListAsync();
    }
}

