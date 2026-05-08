namespace GymSystem.Web.ViewModels.Classes
{
    public class GymClassListViewModel
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
        public string SpotsLeftDisplay => SpotsLeft > 0 ? $"{SpotsLeft} spots left" : "Class Full";
        public string CssClass => SpotsLeft > 0 ? "available" : "full";
    }
}
