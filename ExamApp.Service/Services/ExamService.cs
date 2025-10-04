using ExamApp.Core.Interfaces;
using ExamApp.Core.Models;
using ExamApp.Service.DTOs;
using ExamApp.Service.Interfaces;

namespace ExamApp.Service.Services
{
    public class ExamService : IExamService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ExamService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<QuestionDto>> GetQuestionsAsync()
        {
            var allQuestions = await _unitOfWork.Questions.GetAllAsync();

            // TOPLAM PUAN 100 OLAN SORULAR SEÇ
            var selectedQuestions = SelectQuestionsFor100Points(allQuestions);

            // ŞIKLARI KARIŞTIR
            var questionDtos = new List<QuestionDto>();
            foreach (var question in selectedQuestions)
            {
                questionDtos.Add(ShuffleOptions(question));
            }

            return questionDtos;
        }

        public async Task<ExamResult> SubmitExamAsync(ExamSubmissionDto submission)
        {
            var questions = await _unitOfWork.Questions.GetAllAsync();
            var examQuestions = questions.Where(q => submission.Answers.Keys.Contains(q.Id)).ToList();

            int score = 0;
            int correctCount = 0;
            var wrongAnswers = new List<WrongAnswer>();

            foreach (var answer in submission.Answers)
            {
                var question = examQuestions.FirstOrDefault(q => q.Id == answer.Key);
                if (question != null && !string.IsNullOrEmpty(answer.Value))
                {
                    // DOĞRU CEVABI METİN OLARAK KARŞILAŞTIR - BURASI ÇOK ÖNEMLİ!
                    if (question.CorrectAnswer.Trim().ToLower() == answer.Value.Trim().ToLower())
                    {
                        score += question.Points;
                        correctCount++;
                        Console.WriteLine($"DOĞRU: Soru {question.Id}, Cevap: {answer.Value}, Puan: {question.Points}");
                    }
                    else
                    {
                        wrongAnswers.Add(new WrongAnswer
                        {
                            QuestionId = question.Id,
                            SelectedAnswer = answer.Value,
                            CorrectAnswer = question.CorrectAnswer
                        });
                        Console.WriteLine($"YANLIŞ: Soru {question.Id}, Seçilen: {answer.Value}, Doğru: {question.CorrectAnswer}");
                    }
                }
                else
                {
                    Console.WriteLine($"BOŞ/GEÇERSİZ: Soru {answer.Key}");
                }
            }

            var examResult = new ExamResult
            {
                UserId = submission.UserId,
                Score = score,
                TotalQuestions = examQuestions.Count,
                CorrectAnswers = correctCount,
                ExamDate = DateTime.UtcNow
            };

            await _unitOfWork.ExamResults.AddAsync(examResult);
            await _unitOfWork.CommitAsync();

            foreach (var wrongAnswer in wrongAnswers)
            {
                wrongAnswer.ExamResultId = examResult.Id;
                await _unitOfWork.WrongAnswers.AddAsync(wrongAnswer);
            }
            await _unitOfWork.CommitAsync();

            Console.WriteLine($"SONUÇ: Toplam Puan: {score}, Doğru Sayısı: {correctCount}, Toplam Soru: {examQuestions.Count}");

            return examResult;
        }

        public async Task<ExamResultDto?> GetExamResultAsync(int resultId)
        {
            var result = await _unitOfWork.ExamResults.GetByIdAsync(resultId);
            if (result == null) return null;

            var user = await _unitOfWork.Users.GetByIdAsync(result.UserId);
            if (user == null) return null;

            return new ExamResultDto
            {
                Id = result.Id,
                Username = user.Username,
                Score = result.Score,
                TotalQuestions = result.TotalQuestions,
                CorrectAnswers = result.CorrectAnswers,
                ExamDate = result.ExamDate
            };
        }

        private List<Question> SelectQuestionsFor100Points(IEnumerable<Question> allQuestions)
        {
            var questions = allQuestions.ToList();
            if (questions.Count == 0) return new List<Question>();

            var random = new Random();
            var selectedQuestions = new List<Question>();
            int totalPoints = 0;
            int targetPoints = 100;

            // Soruları karıştır
            questions = questions.OrderBy(x => random.Next()).ToList();

            foreach (var question in questions)
            {
                if (totalPoints + question.Points <= targetPoints)
                {
                    selectedQuestions.Add(question);
                    totalPoints += question.Points;
                }

                if (totalPoints >= targetPoints)
                    break;
            }

            // Eğer hiç soru seçilemediyse, en azından ilk soruyu ekle
            if (selectedQuestions.Count == 0 && questions.Count > 0)
            {
                selectedQuestions.Add(questions.First());
            }

            return selectedQuestions;
        }

        private QuestionDto ShuffleOptions(Question question)
        {
            // Doğru cevabın metnini bul
            var correctAnswerText = question.CorrectAnswer;

            // Tüm şıkları ve harflerini listeye al
            var options = new List<(string Letter, string Text)>
            {
                ("A", question.OptionA),
                ("B", question.OptionB),
                ("C", question.OptionC),
                ("D", question.OptionD)
            };

            // Şıkları karıştır
            var shuffled = options.OrderBy(x => Guid.NewGuid()).ToList();

            return new QuestionDto
            {
                Id = question.Id,
                Text = question.Text,
                OptionA = shuffled[0].Text,
                OptionB = shuffled[1].Text,
                OptionC = shuffled[2].Text,
                OptionD = shuffled[3].Text,
                CorrectAnswer = correctAnswerText, // Doğru cevabın metni DEĞİŞMEZ
                Points = question.Points,
                ShuffledOptions = shuffled.Select(x => x.Text).ToList()
            };
        }
    }
}