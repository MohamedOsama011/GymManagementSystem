

namespace GymSystem.Models.Entities
{
    public class Member
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? PhotoPath { get; set; }

        public int? TrainerId { get; set; }
        public Trainer? Trainer { get; set; }

        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
        public ICollection<ClassEnrollment> ClassEnrollments { get; set; } = new List<ClassEnrollment>();
        public ICollection<AttendanceRecord> AttendanceRecords { get; set; } = new List<AttendanceRecord>();
    }
}
