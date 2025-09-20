using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamApp.Core.Entities;

internal class UserAnswer
{
    public int Id { get; set; }
    public int ExamResultId { get; set; }
    public int QuestionId { get; set; }
    public string SelectedAnswer { get; set; }
    public bool IsCorrect { get; set; }
    public int TimeSpentSeconds { get; set; }

    public virtual ExamResult ExamResult { get; set; }
    public virtual Question Question { get; set; }
}
