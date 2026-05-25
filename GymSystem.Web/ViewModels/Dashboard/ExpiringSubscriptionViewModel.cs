namespace GymSystem.Web.ViewModels.Dashboard
{
    public class ExpiringSubscriptionViewModel
    {
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public string PlanName { get; set; }
        public DateTime EndDate { get; set; }

        // UI Logic
        public int DaysRemaining => Math.Max(0, (EndDate - DateTime.Today).Days);

        public string UrgencyClass => DaysRemaining switch
        {
            0 => "danger",
            <= 3 => "warning",
            _ => "info"
        };

        public string UrgencyText => DaysRemaining switch
        {
            0 => "Today",
            1 => "Tomorrow",
            _ => $"{DaysRemaining} days left"
        };

        public string UrgencyIcon => DaysRemaining switch
        {
            0 => "bi-exclamation-circle-fill",
            <= 3 => "bi-exclamation-triangle-fill",
            _ => "bi-clock-fill"
        };

        public string EndDateFormatted => EndDate.ToString("MMM dd, yyyy");
    }
}
