using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace GymSystem.Web.ViewModels.Classes
{
    public class GymClassDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TrainerName { get; set; }
        public string CategoryName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Capacity { get; set; }
        public int EnrolledCount { get; set; }

        public int SpotsLeft => Capacity - EnrolledCount;
        public string EnrollmentBadgeClass => EnrolledCount >= Capacity ? "bg-danger" : "bg-success";
        public string EnrollmentStatusText => EnrolledCount >= Capacity ? "Full" : "Available";
        public string Duration => (EndTime - StartTime).ToString(@"hh\:mm") + " hours";

        public List<EnrolledMemberViewModel> EnrolledMembers { get; set; } = new();

        public IEnumerable<SelectListItem> AvailableMembers { get; set; } = new List<SelectListItem>();

        [Required(ErrorMessage = "Please select a member")]
        public int SelectedMemberId { get; set; }
    }

    public class EnrolledMemberViewModel
    {
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public string MemberEmail { get; set; }
        public DateTime EnrolledAt { get; set; }
        public string EnrolledSince => EnrolledAt.ToString("MMM dd, yyyy");
        public string MemberAvatar => $"/images/members/{MemberId}.jpg"; 
    }
}

