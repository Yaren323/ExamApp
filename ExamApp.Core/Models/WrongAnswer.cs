namespace ExamApp.Core.Models
{
    public class WrongAnswer
    {
        public int Id { get; set; }

        public int ExamResultId { get; set; }
        public ExamResult ExamResult { get; set; }

        public int QuestionId { get; set; }
        public Question Question { get; set; }

        public string SelectedAnswer { get; set; }
        public string CorrectAnswer { get; set; }
    }
}