using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExamApp.Core.Entities;
using ExamApp.Core.Interfaces;

namespace ExamApp.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IRepository<User> _users;
    private IRepository<Question> _questions;
    private IRepository<ExamResult> _examResults;
    private IRepository<UserAnswer> _userAnswers;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public IRepository<User> Users => _users ??= new Repository<User>(_context);
    public IRepository<Question> Questions => _questions ??= new Repository<Question>(_context);
    public IRepository<ExamResult> ExamResults => _examResults ??= new Repository<ExamResult>(_context);
    public IRepository<UserAnswer> UserAnswers => _userAnswers ??= new Repository<UserAnswer>(_context);

    public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;

    public void Dispose() => _context.Dispose();
}