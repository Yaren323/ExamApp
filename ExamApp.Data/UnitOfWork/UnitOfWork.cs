using ExamApp.Core.Interfaces;
using ExamApp.Core.Models;
using ExamApp.Data.Data;
using ExamApp.Data.Repositories;

namespace ExamApp.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Users = new Repository<User>(_context);
            Questions = new Repository<Question>(_context);
            ExamResults = new Repository<ExamResult>(_context);
            WrongAnswers = new Repository<WrongAnswer>(_context);
        }

        public IRepository<User> Users { get; private set; }
        public IRepository<Question> Questions { get; private set; }
        public IRepository<ExamResult> ExamResults { get; private set; }
        public IRepository<WrongAnswer> WrongAnswers { get; private set; }

        public async Task<int> CommitAsync() => await _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();
    }
}