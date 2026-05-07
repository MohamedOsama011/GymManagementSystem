using GymSystem.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Interfaces
{
    public interface ITrainerService
    {
        Task<IEnumerable<TrainerDto>> GetAllAsync();
        Task<TrainerFormDTO?> GetAsync(int? id = null);
        Task CreateAsync(TrainerFormDTO model);
        Task UpdateAsync(TrainerFormDTO model);
        Task DeleteAsync(int id);
    }
}
