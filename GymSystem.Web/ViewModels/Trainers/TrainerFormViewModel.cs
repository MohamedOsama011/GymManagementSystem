using System.ComponentModel.DataAnnotations;

namespace GymSystem.Web.ViewModels.Trainers
{
    public class TrainerFormViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [Display(Name = "Job Title")]
        public string JobTitle { get; set; } = string.Empty;
    }
}
