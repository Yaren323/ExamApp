namespace ExamApp.Service.DTOs
{
    public class ExamResultDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public int Score { get; set; }
        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; } 
        public DateTime ExamDate { get; set; }

        public double SuccessRate => TotalQuestions > 0 ? (CorrectAnswers * 100.0) / TotalQuestions : 0;
    }
}