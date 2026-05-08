using GymSystem.Models.DTOs;

namespace GymSystem.BLL.Services.Interfaces
{
    public interface ITrainerService
    {
        Task<IEnumerable<TrainerDto>> GetAllAsync();
        Task<TrainerFormDTO?> GetAsync(int? id = null);
        Task<IEnumerable<SpecialtyDto>> GetSpecialtiesLookupAsync();
        Task CreateAsync(TrainerFormDTO model);
        Task UpdateAsync(TrainerFormDTO model);
        Task DeleteAsync(int id);
    }
}
