

namespace GymSystem.Models.Entities
{
    public class Specialty
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<TrainerSpecialty> TrainerSpecialties { get; set; } = new List<TrainerSpecialty>();
    }
}
