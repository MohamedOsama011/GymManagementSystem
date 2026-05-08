using GymSystem.DAL.Data;
using GymSystem.DAL.Repositories.Interfaces;
using GymSystem.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repositories.Implementations
{
    public class TrainerRepository : GenericRepository<Trainer>, ITrainerRepository
    {
        public TrainerRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Trainer>> GetAllWithSpecialtiesAsync()
            => await _dbSet
                .Include(t => t.Members)
                .Include(t => t.TrainerSpecialties)
                    .ThenInclude(ts => ts.Specialty)
                .ToListAsync();

        public async Task<Trainer?> GetWithDetailsAsync(int id)
            => await _dbSet
                .Include(t => t.TrainerSpecialties)
                    .ThenInclude(ts => ts.Specialty)
                .Include(t => t.Members)
                .Include(t => t.GymClasses)
                    .ThenInclude(c => c.Category)
                .FirstOrDefaultAsync(t => t.Id == id);

        public async Task<Trainer?> GetByUserIdAsync(string userId)
            => await _dbSet.FirstOrDefaultAsync(t => t.UserId == userId);
    }
}
