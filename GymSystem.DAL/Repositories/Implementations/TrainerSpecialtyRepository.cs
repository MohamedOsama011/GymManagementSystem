using GymSystem.DAL.Data;
using GymSystem.DAL.Repositories.Interfaces;
using GymSystem.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.DAL.Repositories.Implementations
{
    public class TrainerSpecialtyRepository : GenericRepository<TrainerSpecialty>, ITrainerSpecialtyRepository
    {
        public TrainerSpecialtyRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<TrainerSpecialty>> GetByTrainerIdAsync(int trainerId)
            => await _dbSet
                .Include(ts => ts.Specialty)
                .Where(ts => ts.TrainerId == trainerId)
                .ToListAsync();
    }
}
