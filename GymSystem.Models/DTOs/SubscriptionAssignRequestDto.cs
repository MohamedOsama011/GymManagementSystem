using System;
using System.ComponentModel.DataAnnotations;

namespace GymSystem.Models.DTOs
{
    public class SubscriptionAssignRequestDto
    {
        [Required]
        public int MemberId { get; set; }

        [Required]
        public int PlanId { get; set; }

        public DateTime? StartDate { get; set; }
    }
}
