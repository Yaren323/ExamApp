using ExamApp.Core.Models;
using ExamApp.Service.DTOs;

namespace ExamApp.Service.Interfaces
{
    public interface IUserService
    {
        Task<User> RegisterAsync(RegisterDto registerDto);
        Task<User> LoginAsync(LoginDto loginDto);
        Task<User> GetUserByIdAsync(int id);
        Task<IEnumerable<ExamResult>> GetUserExamResultsAsync(int userId);
    }
}