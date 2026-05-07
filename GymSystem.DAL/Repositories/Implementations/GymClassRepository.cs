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
    public class GymClassRepository : GenericRepository<GymClass>, IGymClassRepository
    {
        public GymClassRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<GymClass>> GetAllWithDetailsAsync()
            => await _dbSet
                .Include(c => c.Trainer)
                .Include(c => c.Category)
                .Include(c => c.ClassEnrollments)
                .ToListAsync();

        public async Task<GymClass?> GetWithDetailsAsync(int id)
            => await _dbSet
                .Include(c => c.Trainer)
                .Include(c => c.Category)
                .Include(c => c.ClassEnrollments)
                    .ThenInclude(ce => ce.Member)
                .FirstOrDefaultAsync(c => c.Id == id);

        public async Task<IEnumerable<GymClass>> GetByTrainerAsync(int trainerId)
            => await _dbSet
                .Include(c => c.Category)
                .Include(c => c.ClassEnrollments)
                .Where(c => c.TrainerId == trainerId)
                .ToListAsync();

        public async Task<int> GetEnrollmentCountAsync(int classId)
            => await _context.ClassEnrollments
                .CountAsync(ce => ce.ClassId == classId);
    }
}

