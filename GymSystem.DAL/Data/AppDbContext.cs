using GymSystem.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.DAL.Data
{
    public class AppDbContext : IdentityDbContext
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TrainerSpecialty>()
                .HasKey(ts => new { ts.TrainerId, ts.SpecialtyId });

            modelBuilder.Entity<ClassEnrollment>()
                .HasKey(ce => new { ce.MemberId, ce.ClassId });

            modelBuilder.Entity<Member>(e =>
            {
                e.Property(m => m.FullName).IsRequired().HasMaxLength(100);
                e.Property(m => m.Email).IsRequired().HasMaxLength(150);
                e.Property(m => m.Phone).HasMaxLength(20);

                e.HasOne(m => m.Trainer)
                 .WithMany(t => t.Members)
                 .HasForeignKey(m => m.TrainerId)
                 .OnDelete(DeleteBehavior.SetNull);
            });
            modelBuilder.Entity<Trainer>(e =>
            {
                e.Property(t => t.FullName).IsRequired().HasMaxLength(100);
                e.Property(t => t.JobTitle).HasMaxLength(100);
            });
            modelBuilder.Entity<Specialty>(e =>
            {
                e.Property(s => s.Name).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<TrainerSpecialty>(e =>
            {
                e.HasOne(ts => ts.Trainer)
                 .WithMany(t => t.TrainerSpecialties)
                 .HasForeignKey(ts => ts.TrainerId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(ts => ts.Specialty)
                 .WithMany(s => s.TrainerSpecialties)
                 .HasForeignKey(ts => ts.SpecialtyId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<MembershipPlan>(e =>
            {
                e.Property(p => p.Name).IsRequired().HasMaxLength(100);
                e.Property(p => p.Price).HasColumnType("decimal(10,2)");
            });

            modelBuilder.Entity<Subscription>(e =>
            {
                e.Property(s => s.Status).IsRequired().HasMaxLength(20);

                e.HasOne(s => s.Member)
                 .WithMany(m => m.Subscriptions)
                 .HasForeignKey(s => s.MemberId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(s => s.Plan)
                 .WithMany(p => p.Subscriptions)
                 .HasForeignKey(s => s.PlanId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ClassCategory>(e =>
            {
                e.Property(c => c.Name).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<GymClass>(e =>
            {
                e.Property(c => c.Name).IsRequired().HasMaxLength(150);

                e.HasOne(c => c.Trainer)
                 .WithMany(t => t.GymClasses)
                 .HasForeignKey(c => c.TrainerId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(c => c.Category)
                 .WithMany(cat => cat.GymClasses)
                 .HasForeignKey(c => c.CategoryId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ClassEnrollment>(e =>
            {
                e.HasOne(ce => ce.Member)
                 .WithMany(m => m.ClassEnrollments)
                 .HasForeignKey(ce => ce.MemberId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(ce => ce.GymClass)
                 .WithMany(c => c.ClassEnrollments)
                 .HasForeignKey(ce => ce.ClassId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<AttendanceRecord>(e =>
            {
                e.HasOne(a => a.Member)
                 .WithMany(m => m.AttendanceRecords)
                 .HasForeignKey(a => a.MemberId)
                 .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
