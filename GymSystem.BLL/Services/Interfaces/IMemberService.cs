using GymSystem.Models.DTOs;

namespace GymSystem.BLL.Services.Interfaces
{
    public interface IMemberService
    {
        Task<IEnumerable<MemberDTO>> GetAllAsync(string? search);
        Task<MemberDTO?> GetByIdAsync(int id);
        Task CreateAsync(MemberCreateDTO dto);
        Task UpdateAsync(MemberUpdateDTO dto);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
