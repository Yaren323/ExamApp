using ExamApp.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace ExamApp.Data.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<ExamResult> ExamResults { get; set; }
        public DbSet<WrongAnswer> WrongAnswers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User configuration
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // UserRole enum'ını integer olarak kaydet
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<int>();

            // ExamResult configuration
            modelBuilder.Entity<ExamResult>()
                .HasOne(er => er.User)
                .WithMany(u => u.ExamResults)
                .HasForeignKey(er => er.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // WrongAnswer configuration
            modelBuilder.Entity<WrongAnswer>()
                .HasOne(wa => wa.ExamResult)
                .WithMany(er => er.WrongAnswers)
                .HasForeignKey(wa => wa.ExamResultId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WrongAnswer>()
                .HasOne(wa => wa.Question)
                .WithMany()
                .HasForeignKey(wa => wa.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}