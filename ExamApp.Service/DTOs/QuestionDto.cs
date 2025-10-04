namespace ExamApp.Service.DTOs
{
    public class QuestionDto
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public string OptionA { get; set; } = string.Empty;
        public string OptionB { get; set; } = string.Empty;
        public string OptionC { get; set; } = string.Empty;
        public string OptionD { get; set; } = string.Empty;
        public string CorrectAnswer { get; set; } = string.Empty;
        public int Points { get; set; }

        // metnı karıstırcaz onun için burayı kontrol ediyoruz.
        public List<string> ShuffledOptions { get; set; } = new();
    }
}