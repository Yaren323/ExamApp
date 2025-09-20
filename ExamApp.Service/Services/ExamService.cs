using ExamApp.Core.Entities;
using ExamApp.Core.Interfaces;
using ExamApp.Service.Interfaces;
namespace ExamApp.Service.Services;


public class ExamService : IExamService
{
    private readonly IUnitOfWork _unitOfWork;

    public ExamService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<Question>> GetAllQuestionsAsync()
    {
        return (await _unitOfWork.Questions.GetAllAsync()).ToList();
    }

    public async Task<Question> GetQuestionByIdAsync(int id)
    {
        return await _unitOfWork.Questions.GetByIdAsync(id);
    }

    public async Task<ExamResult> StartExamAsync(int userId)
    {
        var questions = await GetAllQuestionsAsync();
        var examResult = new ExamResult
        {
            UserId = userId,
            TotalQuestions = questions.Count,
            Score = 0,
            ExamDate = DateTime.UtcNow
        };

        await _unitOfWork.ExamResults.AddAsync(examResult);
        await _unitOfWork.SaveChangesAsync();

        return examResult;
    }

    public async Task<(bool IsCorrect, string CorrectAnswer)> SubmitAnswerAsync(int examResultId, int questionId, string selectedAnswer, int timeSpent)
    {
        var question = await _unitOfWork.Questions.GetByIdAsync(questionId);
        var isCorrect = selectedAnswer == question.CorrectAnswer;

        var userAnswer = new UserAnswer
        {
            ExamResultId = examResultId,
            QuestionId = questionId,
            SelectedAnswer = selectedAnswer,
            IsCorrect = isCorrect,
            TimeSpentSeconds = timeSpent
        };

        await _unitOfWork.UserAnswers.AddAsync(userAnswer);

        if (isCorrect)
        {
            var examResult = await _unitOfWork.ExamResults.GetByIdAsync(examResultId);
            examResult.Score += question.Points;
            _unitOfWork.ExamResults.Update(examResult);
        }

        await _unitOfWork.SaveChangesAsync();

        return (isCorrect, question.CorrectAnswer);
    }

    public async Task<ExamResult> CompleteExamAsync(int examResultId)
    {
        var examResult = await _unitOfWork.ExamResults.GetByIdAsync(examResultId);
        return examResult;
    }

    public async Task<List<ExamResult>> GetUserExamResultsAsync(int userId)
    {
        var allResults = await _unitOfWork.ExamResults.GetAllAsync();
        return allResults.Where(er => er.UserId == userId).ToList();
    }

    public async Task<List<UserAnswer>> GetUserIncorrectAnswersAsync(int userId)
    {
        var examResults = await GetUserExamResultsAsync(userId);
        var examResultIds = examResults.Select(er => er.Id).ToList();

        var allUserAnswers = await _unitOfWork.UserAnswers.GetAllAsync();
        var incorrectAnswers = allUserAnswers
            .Where(ua => examResultIds.Contains(ua.ExamResultId) && !ua.IsCorrect)
            .ToList();

        return incorrectAnswers;
    }
}