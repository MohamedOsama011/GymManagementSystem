using GymSystem.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace GymSystem.DAL.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(AppDbContext context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            // Roles:
            string[] roles = { "Admin", "Trainer" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
            // Admin Users:
            const string adminEmail = "admin@gym.com";
            const string adminPassword = "Admin@1234";

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var admin = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }


            if (!await context.MembershipPlans.AnyAsync())
            {
                context.MembershipPlans.AddRange(
                    new MembershipPlan { Name = "Day Pass",  Price = 50,   DurationInDays = 1   },
                    new MembershipPlan { Name = "Standard",  Price = 300,  DurationInDays = 30  },
                    new MembershipPlan { Name = "Premium", Price = 800, DurationInDays = 90 },
                    new MembershipPlan { Name = "Annual", Price = 2500, DurationInDays = 365 }
                );
            }

            if (!await context.ClassCategories.AnyAsync())
            {
            context.ClassCategories.AddRange(
                new ClassCategory { Name = "Cardio" },
                new ClassCategory { Name = "HIIT" },
                new ClassCategory { Name = "Yoga" },
                new ClassCategory { Name = "Cycling" },
                new ClassCategory { Name = "Boxing" },
                new ClassCategory { Name = "Powerlifting" }
            );
            }

            if (!await context.Specialties.AnyAsync())
            {
                context.Specialties.AddRange(
                    new Specialty { Name = "Yoga" },
                    new Specialty { Name = "Boxing" },
                    new Specialty { Name = "Powerlifting" },
                    new Specialty { Name = "Cardio" },
                    new Specialty { Name = "Nutrition" }
                );
            }

            await context.SaveChangesAsync();
        }
    }
}
