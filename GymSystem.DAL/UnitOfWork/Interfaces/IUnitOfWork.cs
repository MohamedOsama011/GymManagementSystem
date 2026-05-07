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
        ISubscriptionRepository Subscriptions { get; }
        IGymClassRepository GymClasses { get; }
        IAttendanceRepository Attendance { get; }
        Task<int> SaveChangesAsync();
    }
}
