using ExamApp.Core.Models;
using ExamApp.Service.DTOs;

namespace ExamApp.Service.Interfaces
{
    public interface IExamService
    {
        Task<IEnumerable<QuestionDto>> GetQuestionsAsync();
        Task<ExamResult> SubmitExamAsync(ExamSubmissionDto submission);
        Task<ExamResultDto?> GetExamResultAsync(int resultId);
    }
}