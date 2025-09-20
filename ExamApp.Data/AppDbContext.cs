using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExamApp.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExamApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<ExamResult> ExamResults { get; set; }
    public DbSet<UserAnswer> UserAnswers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // İlişkileri konfigure et
        modelBuilder.Entity<ExamResult>()
            .HasOne(er => er.User)
            .WithMany(u => u.ExamResults)
            .HasForeignKey(er => er.UserId);

        modelBuilder.Entity<UserAnswer>()
            .HasOne(ua => ua.ExamResult)
            .WithMany(er => er.UserAnswers)
            .HasForeignKey(ua => ua.ExamResultId);

        modelBuilder.Entity<UserAnswer>()
            .HasOne(ua => ua.Question)
            .WithMany()
            .HasForeignKey(ua => ua.QuestionId);

        // Seed data
        modelBuilder.Entity<Question>().HasData(
            new Question
            {
                Id = 1,
                Text = "ASP.NET Core hangi programlama dilini kullanır?",
                OptionA = "Java",
                OptionB = "C#",
                OptionC = "Python",
                OptionD = "Ruby",
                CorrectAnswer = "B",
                Points = 1,
                TimeLimitSeconds = 60
            },
            new Question
            {
                Id = 2,
                Text = "Entity Framework nedir?",
                OptionA = "Bir programlama dili",
                OptionB = "Bir ORM aracı",
                OptionC = "Bir veritabanı",
                OptionD = "Bir sunucu",
                CorrectAnswer = "B",
                Points = 1,
                TimeLimitSeconds = 60
            }
            // Daha fazla soru ekleyebilirsiniz
        );

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Username = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                Email = "admin@example.com",
                Role = "Admin"
            },
            new User
            {
                Id = 2,
                Username = "user",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("user123"),
                Email = "user@example.com",
                Role = "User"
            }
        );
    }
}