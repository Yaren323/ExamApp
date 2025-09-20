using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamApp.Core.Entities;

internal class ExamResult
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int Score { get; set; }
    public int TotalQuestions { get; set; }
    public DateTime ExamDate { get; set; } = DateTime.UtcNow;

    public virtual User User { get; set; }
    public virtual ICollection<UserAnswer> UserAnswers { get; set; }
}
