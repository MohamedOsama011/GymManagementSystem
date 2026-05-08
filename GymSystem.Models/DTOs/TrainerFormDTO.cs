using System.ComponentModel.DataAnnotations;

namespace GymSystem.Models.DTOs
{
    public class TrainerFormDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string JobTitle { get; set; } = string.Empty;

        public List<int> SelectedSpecialtyIds { get; set; } = new();
    }
}
