using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace GymSystem.Web.ViewModels.Attendance
{
    public class CheckInViewModel
    {
        [Required(ErrorMessage = "Please select a member")]
        [Display(Name = "Member")]
        public int MemberId { get; set; }

        public IEnumerable<SelectListItem> Members { get; set; } = new List<SelectListItem>();

        public int ActiveMembersCount { get; set; }
        public int TotalCheckInsToday { get; set; }
    }
}
