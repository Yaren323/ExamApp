using System.ComponentModel.DataAnnotations;

namespace ExamApp.Core.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public UserRole Role { get; set; } = UserRole.User; 

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<ExamResult> ExamResults { get; set; } = new List<ExamResult>();
    }

    public enum UserRole
    {
        User = 0,
        Admin = 1
    }
}