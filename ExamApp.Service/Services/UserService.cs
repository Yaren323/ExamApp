using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExamApp.Core.Entities;
using ExamApp.Core.Interfaces;
using ExamApp.Service.Interfaces;

namespace ExamApp.Service.Services;


public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<User> AuthenticateAsync(string username, string password)
    {
        var users = await _unitOfWork.Users.GetAllAsync();
        var user = users.FirstOrDefault(u => u.Username == username);

        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            return null;

        return user;
    }

    public async Task<User> GetByIdAsync(int id)
    {
        return await _unitOfWork.Users.GetByIdAsync(id);
    }

    public async Task<User> CreateAsync(User user, string password)
    {
        if (await UserExistsAsync(user.Username))
            throw new Exception("Kullanıcı adı zaten mevcut");

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
        user.CreatedDate = DateTime.UtcNow;

        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return user;
    }

    public async Task<bool> UserExistsAsync(string username)
    {
        var users = await _unitOfWork.Users.GetAllAsync();
        return users.Any(u => u.Username == username);
    }
}