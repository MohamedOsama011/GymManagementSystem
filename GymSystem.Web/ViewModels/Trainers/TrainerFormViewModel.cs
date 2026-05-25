using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace GymSystem.Web.ViewModels.Trainers
{
    public class TrainerFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Full name is required")]
        [StringLength(100)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [Display(Name = "Job Title")]
        public string JobTitle { get; set; } = string.Empty;

        [Display(Name = "Trainer Photo")]
        public IFormFile? Photo { get; set; }

        public string? ExistingPhotoPath { get; set; }

        [Display(Name = "Specialties")]
        public List<int> SelectedSpecialtyIds { get; set; } = new();
        public IEnumerable<SelectListItem> AllSpecialties { get; set; } = new List<SelectListItem>();
    }
}
