using GymSystem.DAL.Data;
using GymSystem.DAL.Repositories.Implementations;
using GymSystem.DAL.Repositories.Interfaces;
using GymSystem.DAL.UnitOfWork.Interfaces;


namespace GymSystem.DAL.UnitOfWork.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IMemberRepository Members { get; private set; }
        public ITrainerRepository Trainers { get; private set; }
        public IMembershipPlanRepository MembershipPlans { get; private set; }

        public ISubscriptionRepository Subscriptions { get; private set; }

        public IGymClassRepository GymClasses { get; private set; }

        public IAttendanceRepository Attendance { get; private set; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;

            Members = new MemberRepository(_context);
            Trainers = new TrainerRepository(_context);
            MembershipPlans = new MembershipPlanRepository(_context);
            Subscriptions = new SubscriptionRepository(_context);
            GymClasses = new GymClassRepository(_context);
            Attendance = new AttendanceRepository(_context);
        }
        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();

        
    }
}
