using GymSystem.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repositories.Interfaces
{
    public interface ITrainerRepository : IGenericRepository<Trainer>
    {
        Task<IEnumerable<Trainer>> GetAllWithSpecialtiesAsync();
        Task<Trainer?> GetWithDetailsAsync(int id);
        Task<Trainer?> GetByUserIdAsync(string userId);
    }
}
