using GymSystem.BLL.Services.Interfaces;
using GymSystem.DAL.UnitOfWork.Interfaces;
using GymSystem.Models.DTOs;
using GymSystem.Models.Entities;

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
            var trainers = await _uow.Trainers.GetAllWithSpecialtiesAsync();

            return trainers
                .OrderBy(t => t.FullName)
                .Select(t => new TrainerDto
                {
                    Id = t.Id,
                    FullName = t.FullName,
                    JobTitle = t.JobTitle
                });
        }

        public async Task<TrainerFormDTO?> GetAsync(int? id = null)
        {
            if (!id.HasValue)
            {
                return new TrainerFormDTO();
            }

            var trainer = await _uow.Trainers.GetWithDetailsAsync(id.Value);
            if (trainer == null)
            {
                return null;
            }

            return new TrainerFormDTO
            {
                Id = trainer.Id,
                FullName = trainer.FullName,
                JobTitle = trainer.JobTitle
            };
        }

        public async Task CreateAsync(TrainerFormDTO model)
        {
            var trainer = new Trainer
            {
                FullName = model.FullName,
                JobTitle = model.JobTitle
            };

            await _uow.Trainers.AddAsync(trainer);
            await _uow.SaveChangesAsync();
        }

        public async Task UpdateAsync(TrainerFormDTO model)
        {
            var trainer = await _uow.Trainers.GetByIdAsync(model.Id);
            if (trainer == null)
            {
                return;
            }

            trainer.FullName = model.FullName;
            trainer.JobTitle = model.JobTitle;

            _uow.Trainers.Update(trainer);
            await _uow.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var trainer = await _uow.Trainers.GetByIdAsync(id);
            if (trainer == null)
            {
                return;
            }

            _uow.Trainers.Delete(trainer);
            await _uow.SaveChangesAsync();
        }
    }
}
