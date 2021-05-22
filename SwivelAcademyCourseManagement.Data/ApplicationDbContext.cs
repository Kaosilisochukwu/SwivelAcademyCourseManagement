using Microsoft.EntityFrameworkCore;
using SwivelAcademyCourseManagement.Domain.Models;
using System;

namespace SwivelAcademyCourseManagement.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>().Property(e => e.Price).HasPrecision(12, 10);
        }
    }
}
