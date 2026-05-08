using GymSystem.Models.Entities;

namespace GymSystem.DAL.Repositories.Interfaces
{
    public interface ITrainerSpecialtyRepository : IGenericRepository<TrainerSpecialty>
    {
        Task<IEnumerable<TrainerSpecialty>> GetByTrainerIdAsync(int trainerId);
    }
}
