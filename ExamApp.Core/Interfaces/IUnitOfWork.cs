using ExamApp.Core.Models;

namespace ExamApp.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> Users { get; }
        IRepository<Question> Questions { get; }
        IRepository<ExamResult> ExamResults { get; }
        IRepository<WrongAnswer> WrongAnswers { get; }

        Task<int> CommitAsync();
    }
}


