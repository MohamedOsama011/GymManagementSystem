namespace GymSystem.Web.ViewModels.Trainers
{
    public class TrainerListViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public string? PhotoPath { get; set; }
        public int MemberCount { get; set; }
        public int ClassCount { get; set; }
        public double WeeklyHours { get; set; }
        public int WeeklyHoursMax { get; set; } = 40;
        public double Rating { get; set; }
        public bool IsActive { get; set; }
        public IEnumerable<string> Specialties { get; set; } = new List<string>();

        public string PrimarySpecialty => Specialties.FirstOrDefault() ?? "General";
        public string RoleBadgeText => string.IsNullOrWhiteSpace(JobTitle)
            ? "COACH"
            : JobTitle.ToUpperInvariant();
        public int WeeklyLoadPercent => WeeklyHoursMax > 0
            ? (int)Math.Min(100, Math.Round(WeeklyHours / WeeklyHoursMax * 100))
            : 0;
        public string WeeklyHoursDisplay => $"{WeeklyHours:0.#} / {WeeklyHoursMax} hrs";
        public string LoadBarColor => WeeklyLoadPercent >= 90
            ? "#e74c3c"
            : WeeklyLoadPercent >= 70
                ? "#e67e22"
                : "var(--primary)";
    }
}
