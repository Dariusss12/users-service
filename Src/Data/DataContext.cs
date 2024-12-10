using Microsoft.EntityFrameworkCore;
using users_service.Src.Models;


namespace users_service.Src.Data
{
    public class DataContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; } = null!;

        public DbSet<Subject> Subjects { get; set; } = null!;

        public DbSet<UserProgress> UsersProgress { get; set; } = null!;

        public DbSet<Career> Careers { get; set; } = null!;
    }
}