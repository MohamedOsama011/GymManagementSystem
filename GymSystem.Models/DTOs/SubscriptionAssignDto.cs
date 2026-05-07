using System.Collections.Generic;

namespace GymSystem.Models.DTOs
{
    public class SubscriptionAssignDto
    {
        public int MemberId { get; set; }
        public string MemberName { get; set; } = string.Empty;
        public SubscriptionDto? CurrentSubscription { get; set; }
        public IEnumerable<MembershipPlanDto> AvailablePlans { get; set; } = new List<MembershipPlanDto>();
    }
}
