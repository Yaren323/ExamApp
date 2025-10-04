namespace ExamApp.Core.Models
{
    public class ExamResult
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;

        public int Score { get; set; }
        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; } 

        public DateTime ExamDate { get; set; } = DateTime.UtcNow;

        public virtual ICollection<WrongAnswer> WrongAnswers { get; set; } = new List<WrongAnswer>();
    }
}