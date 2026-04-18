using GymSystem.BLL.Services.Interfaces;
using GymSystem.DAL.UnitOfWork.Interfaces;
using GymSystem.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Implementations
{
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork _uow;
        public TrainerService(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public async Task<IEnumerable<TrainerDto>> GetAllAsync()
        {
            var trainers = await _uow.Trainers.GetAllWithDetailsAsync();

            return trainers.Select(t => new TrainerDto
            {
                FullName = t.FullName,
                Id = t.Id,
            });
        }
    }
}
