using GymSystem.BLL.Services.Interfaces;
using GymSystem.DAL.UnitOfWork.Implementations;
using GymSystem.DAL.UnitOfWork.Interfaces;
using GymSystem.Models.DTOs;
using GymSystem.Models.Entities;

namespace GymSystem.BLL.Services.Implementations
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _uow;

        public MemberService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<MemberDTO>> GetAllAsync(string? search)
        {
            var members = await _uow.Members.GetAllWithDetailsAsync();

            if (!string.IsNullOrEmpty(search))
            {
                members = members.Where(m => m.FullName.Contains(search));
            }

            return members.Select(m => new MemberDTO
            {
                Id = m.Id,
                FullName = m.FullName,
                Email = m.Email,
                Phone = m.Phone,
                TrainerName = m.Trainer?.FullName ?? "No Trainer"
            });
        }
        public async Task<MemberDTO?> GetByIdAsync(int id)
        {
            var member = await _uow.Members.GetByIdAsync(id);
            if (member == null) return null;

            return new MemberDTO
            {
                Id = member.Id,
                FullName = member.FullName,
                Email = member.Email,
                Phone = member.Phone,
                TrainerName = member.Trainer?.FullName ?? "No Trainer"
            };
        }

        public async Task CreateAsync(MemberCreateDTO dto)
        {
            var members = new Member
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Phone = dto.Phone,
                DateOfBirth = dto.DateOfBirth,
                TrainerId = dto.TrainerId
            };

            await _uow.Members.AddAsync(members);
            await _uow.SaveChangesAsync();
        }
        public async Task UpdateAsync(MemberUpdateDTO dto)
        {
            var member = await _uow.Members.GetByIdAsync(dto.Id);

            if (member != null)
            {
                member.FullName = dto.FullName;
                member.Email = dto.Email;
                member.Phone = dto.Phone;
                member.DateOfBirth = (DateTime)dto.DateOfBirth;
                member.TrainerId = dto.TrainerId;

                _uow.Members.Update(member);
                await _uow.SaveChangesAsync();

            }

        }
        public async Task DeleteAsync(int id)
        {
            var member = await _uow.Members.GetByIdAsync(id);
            if (member != null)
            {
                _uow.Members.Delete(member);
                await _uow.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            var member = await _uow.Members.GetByIdAsync(id);
            return member != null;
        }

        
    }
}
