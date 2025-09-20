using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamApp.Core.Entities;

internal class Question
{
    public int Id { get; set; }
    public string Text { get; set; }
    public string OptionA { get; set; }
    public string OptionB { get; set; }
    public string OptionC { get; set; }
    public string OptionD { get; set; }
    public string CorrectAnswer { get; set; } // A, B, C veya D
    public int Points { get; set; } = 1;
    public int TimeLimitSeconds { get; set; } = 60;
}
