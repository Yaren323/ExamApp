using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamApp.Core.Entities;

internal class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string Email { get; set; }
    public string Role { get; set; } = "User";
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public virtual ICollection<ExamResult> ExamResults { get; set; }
    public virtual ICollection<UserAnswer> UserAnswers { get; set; }
}
