namespace GymSystem.Web.ViewModels.Members
{
    public class MemberDetailsViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? PhotoPath { get; set; }
        public string? TrainerName { get; set; }
        public int? TrainerId { get; set; }
        //public SubscriptionStatusViewModel? ActiveSubscription { get; set; }
        //public IEnumerable<SubscriptionStatusViewModel> SubscriptionHistory { get; set; } = new List<SubscriptionStatusViewModel>();

    }
}
