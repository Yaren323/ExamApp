using ExamApp.Core.Entities;
using ExamApp.Core.Interfaces;
using ExamApp.Service.Interfaces;
namespace ExamApp.Service.Interfaces;
public interface IExamService
{
    Task<List<Question>> GetAllQuestionsAsync();
    Task<Question> GetQuestionByIdAsync(int id);
    Task<ExamResult> StartExamAsync(int userId);
    Task<(bool IsCorrect, string CorrectAnswer)> SubmitAnswerAsync(int examResultId, int questionId, string selectedAnswer, int timeSpent);
    Task<ExamResult> CompleteExamAsync(int examResultId);
    Task<List<ExamResult>> GetUserExamResultsAsync(int userId);
    Task<List<UserAnswer>> GetUserIncorrectAnswersAsync(int userId);
}