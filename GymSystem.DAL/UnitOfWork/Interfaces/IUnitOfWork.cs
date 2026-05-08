using GymSystem.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.UnitOfWork.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IMemberRepository Members { get; }
        ITrainerRepository Trainers { get; }
        IMembershipPlanRepository MembershipPlans { get; }
        ISpecialtyRepository Specialties { get; }
        ITrainerSpecialtyRepository TrainerSpecialties { get; }
        ISubscriptionRepository Subscriptions { get; }
        IGymClassRepository GymClasses { get; }
        IAttendanceRepository Attendance { get; }
        IGenericRepository<T> Repository<T>() where T : class;

        Task<int> SaveChangesAsync();
    }
}
