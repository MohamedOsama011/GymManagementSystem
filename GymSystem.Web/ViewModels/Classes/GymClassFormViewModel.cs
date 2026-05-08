using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace GymSystem.Web.ViewModels.Classes
{
    public class GymClassFormViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Trainer")]
        public int TrainerId { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Required]
        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "End Time")]
        public DateTime EndTime { get; set; } = DateTime.Now.AddHours(1);

        [Required]
        [Range(1, 200)]
        public int Capacity { get; set; } = 20;

        public IEnumerable<SelectListItem> Trainers { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
    }
}
