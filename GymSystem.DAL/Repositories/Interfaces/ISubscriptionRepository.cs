using GymSystem.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repositories.Interfaces
{
    public interface ISubscriptionRepository
    {
        Task<Subscription?> GetActivSubscriptionAsync(int memberId);
        Task<IEnumerable<Subscription>> GetExpiringSoonAsync(int daysAhead);
        Task<IEnumerable<Subscription>> GetByMemberAsync(int memberId);
    }
}
