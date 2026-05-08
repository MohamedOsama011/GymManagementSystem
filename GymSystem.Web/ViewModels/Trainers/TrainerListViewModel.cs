namespace GymSystem.Web.ViewModels.Trainers
{
    public class TrainerListViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public int MemberCount { get; set; }
        public IEnumerable<string> Specialties { get; set; } = new List<string>();
    }
}
