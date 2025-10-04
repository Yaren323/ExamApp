using ExamApp.Core.Interfaces;
using ExamApp.Core.Models;
using ExamApp.Service.Interfaces;

namespace ExamApp.Service.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Question> AddQuestionAsync(Question question)
        {
            question.CreatedAt = DateTime.UtcNow;
            await _unitOfWork.Questions.AddAsync(question);
            await _unitOfWork.CommitAsync();
            return question;
        }

        public async Task<bool> UpdateQuestionAsync(Question question)
        {
            var existingQuestion = await _unitOfWork.Questions.GetByIdAsync(question.Id);
            if (existingQuestion == null) return false;

            existingQuestion.Text = question.Text;
            existingQuestion.OptionA = question.OptionA;
            existingQuestion.OptionB = question.OptionB;
            existingQuestion.OptionC = question.OptionC;
            existingQuestion.OptionD = question.OptionD;
            existingQuestion.CorrectAnswer = question.CorrectAnswer;
            existingQuestion.Points = question.Points;

            _unitOfWork.Questions.Update(existingQuestion);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public async Task<bool> DeleteQuestionAsync(int questionId)
        {
            var question = await _unitOfWork.Questions.GetByIdAsync(questionId);
            if (question == null) return false;

            _unitOfWork.Questions.Remove(question);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public async Task<IEnumerable<Question>> GetAllQuestionsAsync()
        {
            return await _unitOfWork.Questions.GetAllAsync();
        }

        public async Task<Question?> GetQuestionByIdAsync(int id)
        {
            return await _unitOfWork.Questions.GetByIdAsync(id);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _unitOfWork.Users.GetAllAsync();
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null) return false;

            _unitOfWork.Users.Remove(user);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}