using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GymSystem.Models.DTOs;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymSystem.Web.ViewModels.Subscriptions
{
    public class SubscriptionAssignViewModel
    {
        public int MemberId { get; set; }

        public string MemberName { get; set; } = string.Empty;

        public SubscriptionDto? CurrentSubscription { get; set; }

        public IEnumerable<SelectListItem> AvailablePlans { get; set; } = new List<SelectListItem>();

        [Required]
        [Display(Name = "Membership Plan")]
        public int SelectedPlanId { get; set; }

        [Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }
    }
}
