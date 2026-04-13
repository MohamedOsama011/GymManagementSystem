
namespace GymSystem.Models.Entities
{
    public class Trainer
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string JobTitle { get; set; }
        public string? UserId { get; set; }

        public ICollection<Member> Members { get; set; } = new List<Member>();
        public ICollection<TrainerSpecialty> TrainerSpecialties { get; set; } = new List<TrainerSpecialty>();
        public ICollection<GymClass> GymClasses { get; set; } = new List<GymClass>();
    }
}
