using GymSystem.BLL.Services.Interfaces;
using GymSystem.DAL.UnitOfWork.Interfaces;
using GymSystem.Models.DTOs;
using GymSystem.Models.Entities;
using System.Collections.Generic;
using System.Linq;
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
            var trainers = await _uow.Trainers.GetAllWithSpecialtiesAsync();

            return trainers
                .OrderBy(t => t.FullName)
                .Select(t => new TrainerDto
                {
                    Id = t.Id,
                    FullName = t.FullName,
                    JobTitle = t.JobTitle,
                    MemberCount = t.Members.Count,
                    Specialties = t.TrainerSpecialties
                        .Select(ts => ts.Specialty.Name)
                        .OrderBy(name => name)
                        .ToList()
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
                JobTitle = trainer.JobTitle,
                SelectedSpecialtyIds = trainer.TrainerSpecialties
                    .Select(ts => ts.SpecialtyId)
                    .ToList()
            };
        }

        public async Task<IEnumerable<SpecialtyDto>> GetSpecialtiesLookupAsync()
        {
            var specialties = await _uow.Specialties.GetAllAsync();
            return specialties
                .OrderBy(s => s.Name)
                .Select(s => new SpecialtyDto
                {
                    Id = s.Id,
                    Name = s.Name
                })
                .ToList();
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

            await SyncTrainerSpecialtiesAsync(trainer.Id, model.SelectedSpecialtyIds);
        }

        public async Task UpdateAsync(TrainerFormDTO model)
        {
            var trainer = await _uow.Trainers.GetWithDetailsAsync(model.Id);
            if (trainer == null)
            {
                return;
            }

            trainer.FullName = model.FullName;
            trainer.JobTitle = model.JobTitle;

            _uow.Trainers.Update(trainer);
            await SyncTrainerSpecialtiesAsync(trainer.Id, model.SelectedSpecialtyIds);
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

        private async Task SyncTrainerSpecialtiesAsync(int trainerId, IEnumerable<int> selectedSpecialtyIds)
        {
            selectedSpecialtyIds ??= Enumerable.Empty<int>();

            var existing = await _uow.TrainerSpecialties.GetByTrainerIdAsync(trainerId);
            foreach (var trainerSpecialty in existing)
            {
                _uow.TrainerSpecialties.Delete(trainerSpecialty);
            }

            foreach (var specialtyId in selectedSpecialtyIds.Distinct())
            {
                await _uow.TrainerSpecialties.AddAsync(new TrainerSpecialty
                {
                    TrainerId = trainerId,
                    SpecialtyId = specialtyId
                });
            }
        }
    }
}
