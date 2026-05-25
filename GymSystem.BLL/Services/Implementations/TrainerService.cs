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
            var weekStart = DateTime.Today.AddDays(-6);
            var weekEnd = DateTime.Today.AddDays(1);

            return trainers
                .OrderBy(t => t.FullName)
                .Select(t =>
                {
                    var weeklyClasses = t.GymClasses
                        .Where(c => c.StartTime >= weekStart && c.StartTime < weekEnd)
                        .ToList();
                    var weeklyHours = weeklyClasses.Sum(c => (c.EndTime - c.StartTime).TotalHours);
                    var classCount = t.GymClasses.Count;
                    var memberCount = t.Members.Count;

                    return new TrainerDto
                    {
                        Id = t.Id,
                        FullName = t.FullName,
                        JobTitle = t.JobTitle,
                        PhotoPath = t.PhotoPath,
                        MemberCount = memberCount,
                        ClassCount = classCount,
                        WeeklyHours = Math.Round(weeklyHours, 1),
                        WeeklyHoursMax = 40,
                        Rating = Math.Round(Math.Min(5.0, 4.2 + memberCount * 0.04 + classCount * 0.02), 1),
                        IsActive = weeklyHours > 0 || memberCount > 0,
                        Specialties = t.TrainerSpecialties
                            .Select(ts => ts.Specialty.Name)
                            .OrderBy(name => name)
                            .ToList()
                    };
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
                PhotoPath = trainer.PhotoPath,
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
                JobTitle = model.JobTitle,
                PhotoPath = model.PhotoPath
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
            trainer.PhotoPath = model.PhotoPath;

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
