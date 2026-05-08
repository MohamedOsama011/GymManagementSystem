using GymSystem.DAL.Data;
using GymSystem.DAL.Repositories.Interfaces;
using GymSystem.Models.Entities;

namespace GymSystem.DAL.Repositories.Implementations
{
    public class SpecialtyRepository : GenericRepository<Specialty>, ISpecialtyRepository
    {
        public SpecialtyRepository(AppDbContext context) : base(context)
        {
        }
    }
}
