using GymSystem.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repositories.Interfaces
{
    public interface IGymClassRepository : IGenericRepository<GymClass>
    {
        Task<IEnumerable<GymClass>> GetAllWithDetailsAsync();
        Task<GymClass?> GetWithDetailsAsync(int id);
        Task<IEnumerable<GymClass>> GetByTrainerAsync(int trainerId);
        Task<int> GetEnrollmentCountAsync(int classId);
    }
}
