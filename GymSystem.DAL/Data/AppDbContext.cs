using GymSystem.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.DAL.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Member> Members { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Specialty> Specialties { get; set; }
        public DbSet<TrainerSpecialty> TrainerSpecialties { get; set; }
        public DbSet<MembershipPlan> MembershipPlans { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<ClassCategory> ClassCategories { get; set; }
        public DbSet<GymClass> GymClasses { get; set; }
        public DbSet<ClassEnrollment> ClassEnrollments { get; set; }
        public DbSet<AttendanceRecord> AttendanceRecords { get; set; }
    }
}
