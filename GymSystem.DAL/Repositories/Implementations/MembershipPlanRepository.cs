using GymSystem.DAL.Data;
using GymSystem.DAL.Repositories.Interfaces;
using GymSystem.Models.Entities;

namespace GymSystem.DAL.Repositories.Implementations
{
    public class MembershipPlanRepository : GenericRepository<MembershipPlan>, IMembershipPlanRepository
    {
        public MembershipPlanRepository(AppDbContext context) : base(context)
        {
        }
    }
}
