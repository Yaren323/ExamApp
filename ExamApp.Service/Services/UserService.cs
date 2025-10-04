using ExamApp.Core.Interfaces;
using ExamApp.Core.Models;
using ExamApp.Service.DTOs;
using ExamApp.Service.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace ExamApp.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<User> RegisterAsync(RegisterDto registerDto)
        {
            // Kullanıcı adı ve email kontrolü
            var existingUser = await _unitOfWork.Users.SingleOrDefaultAsync(u =>
                u.Username == registerDto.Username || u.Email == registerDto.Email);

            if (existingUser != null)
                throw new Exception("Kullanıcı adı veya email zaten kullanımda");

            // Şifre hashleme - TUTARLI BİR YÖNTEM
            var hashedPassword = HashPassword(registerDto.Password);

            var user = new User
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                PasswordHash = hashedPassword,
                Role = UserRole.User,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CommitAsync();

            return user;
        }

        public async Task<User?> LoginAsync(LoginDto loginDto)
        {
            var user = await _unitOfWork.Users.SingleOrDefaultAsync(u => u.Username == loginDto.Username);

            if (user == null)
                return null;

            // Şifre doğrulama - AYNI HASH YÖNTEMİNİ KULLAN
            var hashedPassword = HashPassword(loginDto.Password);

            if (user.PasswordHash != hashedPassword)
                return null;

            return user;
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _unitOfWork.Users.GetByIdAsync(id);
        }

        public async Task<IEnumerable<ExamResult>> GetUserExamResultsAsync(int userId)
        {
            return await _unitOfWork.ExamResults.FindAsync(er => er.UserId == userId);
        }

        private string HashPassword(string password)
        {
            // TUTARLI BİR HASH YÖNTEMİ - SHA256
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}