namespace GymSystem.Web.ViewModels.Members
{
    public class MemberListViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string? TrainerName { get; set; }
        public string? ActivePlanName { get; set; }
        public string SubscriptionStatus { get; set; } = "None";
    }
}
