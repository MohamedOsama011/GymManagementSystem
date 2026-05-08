using GymSystem.BLL.Services.Interfaces;
using GymSystem.DAL.UnitOfWork.Interfaces;
using GymSystem.Models.DTOs;
using GymSystem.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Implementations
{
    public class GymClassService : IGymClassService
    {
        private readonly IUnitOfWork _uow;

        public GymClassService(IUnitOfWork uow) => _uow = uow;

        public async Task<IEnumerable<GymClassListDto>> GetAllAsync()
        {
            var classes = await _uow.GymClasses.GetAllWithDetailsAsync();

            
            return classes.Select(c => new GymClassListDto
            {
                Id = c.Id,
                Name = c.Name,
                TrainerName = c.Trainer.FullName,
                CategoryName = c.Category.Name,
                StartTime = c.StartTime,
                EndTime = c.EndTime,
                Capacity = c.Capacity,
                EnrolledCount = c.ClassEnrollments.Count
            });
        }

        public async Task<GymClassFormDto> GetFormDataAsync(int? id = null)
        {
            var trainers = await _uow.Trainers.GetAllAsync();
            
            var categories = await _uow.Repository<ClassCategory>().GetAllAsync();

            var trainerDtos = trainers.Select(t => new LookupItemDto
            {
                Id = t.Id,
                Name = t.FullName
            }).ToList();

            var categoryDtos = categories.Select(c => new LookupItemDto
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();

            if (id == null)
                return new GymClassFormDto
                {
                    Trainers = trainerDtos,
                    Categories = categoryDtos
                };

            var gymClass = await _uow.GymClasses.GetWithDetailsAsync(id.Value);
            if (gymClass == null)
                return new GymClassFormDto
                {
                    Trainers = trainerDtos,
                    Categories = categoryDtos
                };

            return new GymClassFormDto
            {
                Id = gymClass.Id,
                Name = gymClass.Name,
                TrainerId = gymClass.TrainerId,
                CategoryId = gymClass.CategoryId,
                StartTime = gymClass.StartTime,
                EndTime = gymClass.EndTime,
                Capacity = gymClass.Capacity,
                Trainers = trainerDtos,
                Categories = categoryDtos
            };
        }

        public async Task CreateAsync(GymClassFormDto dto)
        {
            var gymClass = new GymClass
            {
                Name = dto.Name,
                TrainerId = dto.TrainerId,
                CategoryId = dto.CategoryId,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                Capacity = dto.Capacity
            };

            await _uow.GymClasses.AddAsync(gymClass);
            await _uow.SaveChangesAsync();
        }

        public async Task UpdateAsync(GymClassFormDto dto)
        {
            var gymClass = await _uow.GymClasses.GetByIdAsync(dto.Id);
            if (gymClass == null)
                throw new KeyNotFoundException($"Class with ID {dto.Id} not found");

            gymClass.Name = dto.Name;
            gymClass.TrainerId = dto.TrainerId;
            gymClass.CategoryId = dto.CategoryId;
            gymClass.StartTime = dto.StartTime;
            gymClass.EndTime = dto.EndTime;
            gymClass.Capacity = dto.Capacity;

            _uow.GymClasses.Update(gymClass);
            await _uow.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var gymClass = await _uow.GymClasses.GetByIdAsync(id);
            if (gymClass == null)
                throw new KeyNotFoundException($"Class with ID {id} not found");

            _uow.GymClasses.Delete(gymClass);
            await _uow.SaveChangesAsync();
        }
        public async Task<GymClassDetailsDto> GetDetailsAsync(int id)
        {
            var gymClass = await _uow.GymClasses.GetWithDetailsAsync(id);

            if (gymClass == null)
                return null;

            var allMembers = await _uow.Members.GetAllAsync();

            var enrolledMemberIds = gymClass.ClassEnrollments.Select(ce => ce.MemberId).ToHashSet();

            var availableMembers = allMembers
                .Where(m => !enrolledMemberIds.Contains(m.Id))
                .Select(m => new LookupItemDto
                {
                    Id = m.Id,
                    Name = m.FullName
                })
                .ToList();

            return new GymClassDetailsDto
            {
                Id = gymClass.Id,
                Name = gymClass.Name,
                TrainerName = gymClass.Trainer?.FullName,
                CategoryName = gymClass.Category?.Name,
                StartTime = gymClass.StartTime,
                EndTime = gymClass.EndTime,
                Capacity = gymClass.Capacity,
                EnrolledCount = gymClass.ClassEnrollments.Count,
                EnrolledMembers = gymClass.ClassEnrollments.Select(ce => new EnrolledMemberDto
                {
                    MemberId = ce.MemberId,
                    MemberName = ce.Member?.FullName,
                    MemberEmail = ce.Member?.Email,
                    EnrolledAt = ce.EnrolledAt
                }).ToList(),
                AvailableMembers = availableMembers
            };
        }

        public async Task<EnrollmentResultDto> EnrollMemberAsync(int classId, int memberId)
        {
            var gymClass = await _uow.GymClasses.GetWithDetailsAsync(classId);
            if (gymClass == null)
                return new EnrollmentResultDto
                {
                    Success = false,
                    Message = "Class not found"
                };

            var alreadyEnrolled = gymClass.ClassEnrollments.Any(ce => ce.MemberId == memberId);
            if (alreadyEnrolled)
                return new EnrollmentResultDto
                {
                    Success = false,
                    Message = "Member already enrolled"
                };

            var spotsLeft = gymClass.Capacity - gymClass.ClassEnrollments.Count;
            if (spotsLeft <= 0)
                return new EnrollmentResultDto
                {
                    Success = false,
                    Message = "Class is full"
                };

            await _uow.Repository<ClassEnrollment>().AddAsync(new ClassEnrollment
            {
                ClassId = classId,
                MemberId = memberId,
                EnrolledAt = DateTime.Now
            });

            await _uow.SaveChangesAsync();

            return new EnrollmentResultDto
            {
                Success = true,
                Message = "Enrollment successful"
            };
        }

        public async Task<EnrollmentResultDto> UnenrollMemberAsync(int classId, int memberId)
        {
            var enrollment = (await _uow.Repository<ClassEnrollment>()
                .FindAsync(ce => ce.ClassId == classId && ce.MemberId == memberId))
                .FirstOrDefault();

            if (enrollment == null)
                return new EnrollmentResultDto
                {
                    Success = false,
                    Message = "Enrollment not found"
                };

            _uow.Repository<ClassEnrollment>().Delete(enrollment);
            await _uow.SaveChangesAsync();

            return new EnrollmentResultDto
            {
                Success = true,
                Message = "Unenrollment successful"
            };
        }
        
        
    }
}
