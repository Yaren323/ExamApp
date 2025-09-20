using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExamApp.Core.Entities;

namespace ExamApp.Core.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IRepository<User> Users { get; }
    IRepository<Question> Questions { get; }
    IRepository<ExamResult> ExamResults { get; }
    IRepository<UserAnswer> UserAnswers { get; }
    Task<bool> SaveChangesAsync();
}
