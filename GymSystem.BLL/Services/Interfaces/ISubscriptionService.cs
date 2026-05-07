
using GymSystem.Models.DTOs;

namespace GymSystem.BLL.Services.Interfaces
{
    public interface ISubscriptionService
    {
        Task<SubscriptionAssignDto?> GetAssignAsync(int memberId);
        Task AssignAsync(SubscriptionAssignRequestDto dto);
        Task<IEnumerable<SubscriptionDto>> GetExpiringSoonAsync(int daysAhead = 7);
    }
}
