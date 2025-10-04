namespace ExamApp.Service.DTOs
{
    public class ExamSubmissionDto
    {
        public int UserId { get; set; }
        public Dictionary<int, string> Answers { get; set; } = new(); 
    }
}