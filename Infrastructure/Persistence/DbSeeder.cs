using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public static class DbSeeder
    {
        public static async System.Threading.Tasks.Task SeedAsync(AppDbContext context,IPasswordHasher passwordHasher)
        {
            if (!await context.Users.AnyAsync())
            {
                var users = new List<User>
                {
                    new User
                    {
                        FullName = "Admin User",
                        Email = "admin@demo.com",
                        Password = passwordHasher.Hash("Admin123!"),
                        Role = UserRole.Admin,
                    },
                    new User
                    {
                        FullName = "Manager User",
                        Email = "manager@demo.com",
                        Password = passwordHasher.Hash("Manager123!"),
                        Role = UserRole.Manager,
                    },
                    new User
                    {
                        FullName = "Employee User",
                        Email = "employee@demo.com",
                        Password = passwordHasher.Hash("Employee123!"),
                        Role = UserRole.Employee,
                    }
                };

                await context.Users.AddRangeAsync(users);
                await context.SaveChangesAsync();
            }
        }
    }
}
