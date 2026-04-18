using GymSystem.Models.Entities;

namespace GymSystem.DAL.Repositories.Interfaces
{
    public interface IMemberRepository : IGenericRepository<Member>
    {
        Task<IEnumerable<Member>> GetAllWithDetailsAsync();
        Task<Member?> GetWithDetailsAsync(int id);
        Task<IEnumerable<Member>> GetByTrainerAsync(int trainerId);
    }
}
