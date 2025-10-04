using System.ComponentModel.DataAnnotations;

namespace ExamApp.Core.Models
{
    public class Question
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Soru metni gereklidir")]
        [StringLength(500, ErrorMessage = "Soru metni 500 karakterden uzun olamaz")]
        public string Text { get; set; } = string.Empty;

        [Required(ErrorMessage = "A şıkkı gereklidir")]
        public string OptionA { get; set; } = string.Empty;

        [Required(ErrorMessage = "B şıkkı gereklidir")]
        public string OptionB { get; set; } = string.Empty;

        [Required(ErrorMessage = "C şıkkı gereklidir")]
        public string OptionC { get; set; } = string.Empty;

        [Required(ErrorMessage = "D şıkkı gereklidir")]
        public string OptionD { get; set; } = string.Empty;

        [Required(ErrorMessage = "Doğru cevap gereklidir")]
        public string CorrectAnswer { get; set; } = string.Empty;

        [Range(1, 100, ErrorMessage = "Puan 1-100 arasında olmalıdır")]
        public int Points { get; set; } = 10;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}